using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Model {
    // the set of semantic values in a model M.
    // It's queried for the truth value of sentences,
    // and the referents of expressions that refer to
    // individuals or functions
    private Dictionary<ISemanticType, Dictionary<int, ISemanticValue>> model;

    // the fields for rules:
    // the rules written in compact form with free variables.
    private Dictionary<ISemanticType, HashSet<Rule>> formulaRules;
    
    // the rules expanded to form sentences
    // with variables bound to constants for
    // each possible semantic value in the given domain.
    private HashSet<Rule> sentenceRules;
    
    // the rules that have been used to support an inference.
    // They are not checked each time the model updates, until
    // there is an inconsistency which puts it back in play.
    // Also, it's used during inconsistency resolution as a way
    // of tracking the justification chains of a given proposition
    private HashSet<Rule> activeRules;

    // the model which this model inherets from.
    // 1. Denotation(): if not defined in a lower model, should look higher
    // 2. Satisfies(): if not in lower model (for P and -P), then look higher
    // 3. Make()/Update(): only affect lowest level called
    Model super;

    // this is the default ID number for a wrapped
    // logical form with any semantic type.
    public static int WRAPPERS_ID = 5;

    // the constructor
    public Model(Model super) {
        this.model = new Dictionary<ISemanticType, Dictionary<int, ISemanticValue>>();
        this.formulaRules = new Dictionary<ISemanticType, HashSet<Rule>>();
        this.sentenceRules = new HashSet<Rule>();
        this.activeRules = new HashSet<Rule>();
        this.super = super;
    }

    // defaults to having no super model
    // if no super model argument is provided
    public Model() : this(null) {}

    // simple getter
    public Model GetSuperModel() {
        return super;
    }

    // Adds a semantic value to the model, associating both
    // a semantic type and an ID number to it.
    public UpdateInfo Add(ISemanticType t, int id, ISemanticValue v) {
        // if the semantic type isn't currently in the model,
        // add an entry for it.
        if (!model.ContainsKey(t)) {
            model[t] = new Dictionary<int, ISemanticValue>();
        }
        // get all the semantic values in the model with the
        // semantic type, t.
        Dictionary<int, ISemanticValue> modelByType = model[t];
        // if the ID is already defined for the semantic type t,
        // then don't change the model at all!
        if (modelByType.ContainsKey(id)) {
            return UpdateInfo.NoChange;
        }
        // otherwise, set the ID to the semantic value.
        modelByType[id] = v;

        // if there are formula rules with the given semantic type,
        // then we want to generate a sentence rule that refers
        // to the new semantic value.
        if (formulaRules.ContainsKey(t)) {
            foreach (Rule fr in formulaRules[t]) {
                HashSet<Rule> srs = GetSentenceRules(fr);
                foreach (Rule sr in srs) {
                    sentenceRules.Add(sr);
                }
            }
        }
        // if we're here, then we've updated the model!
        return UpdateInfo.Updated;
    }

    // generates all the sentence-rules from a formula-rule
    public HashSet<Rule> GetSentenceRules(Rule r) {
        HashSet<Rule> sentenceRules = new HashSet<Rule>();

        // if the rule is closed, then it's all set!
        if (r.IsClosed()) {
            sentenceRules.Add(r);
            return sentenceRules;
        }

        HashSet<Variable> freeVariables = r.GetFreeVariables();

        foreach (Variable v in freeVariables) {
            // TODO optimize by storing domain
            // the IDs of all the semantic values
            // with the same semantic type as v
            HashSet<int> ids = GetDomain(v.GetSemanticType());
            // go through these and make all the sentence rules out of it.
            foreach (int id in ids) {
                Rule boundRule = r.Bind(v.GetID(), new Constant(v.GetSemanticType(), id));
                sentenceRules.UnionWith(GetSentenceRules(boundRule));
            }
        }

        return sentenceRules;
    }

    // Adds a rule, r, to the model.
    // 
    public void Add(Rule r) {
        if (r.IsClosed()) {
            sentenceRules.Add(r);
        } else {
            HashSet<Variable> variables = r.GetFreeVariables();
            foreach (Variable v in variables) {
                if (!formulaRules.ContainsKey(v.GetSemanticType())) {
                    formulaRules.Add(v.GetSemanticType(), new HashSet<Rule>());
                }
                formulaRules[v.GetSemanticType()].Add(r);
                // now we expand the formula rule
                // into a bunch of sentence rules
                
                foreach (Rule sr in GetSentenceRules(r)) {
                    sentenceRules.Add(r);
                }
            }
        }
    }

    public ISemanticValue Get(ISemanticType t, int id) {
        return model[t][id];
    }
    
    // super compatible
    public bool Satisfies(LogicalForm l) {
        if (l.IsClosed() && l.IsFormula()) {
            ISemanticValue s = l.Denotation(this);
            if (s.GetType() == typeof(TruthValue)) {
                TruthValue t = (TruthValue) s;
                if (t.IsUnknown() && (super != null)) {
                    return super.Satisfies(l);
                }
                return t.IsTrue();
            }
            if (super != null) {
                return super.Satisfies(l);
            }
            return false;
        }
        return false;
    }
    
    // Updates the Model (M) such that
    // l has the truth value v in M
    private UpdateInfo Make(LogicalForm l, TruthValue.T v) {
        if (l.IsClosed() && l.IsFormula()) {
            return UpdateInfo.NoChange;
        }
        return l.Make(this, v);
    }
    
    // Makes s true in M
    public UpdateInfo MakeTrue(LogicalForm s) {
        return Make(s, TruthValue.T.True);
    }

    // makes the inference prescribed by r in this model
    public UpdateInfo MakeInference(Rule r) {
        if (!r.IsClosed()) {
            return UpdateInfo.NoChange;
        }

        LogicalForm inference = r.GetInference(this);

        if (inference != null) {
            sentenceRules.Remove(r);
            activeRules.Add(r);
            return MakeTrue(inference);    
        }

        return UpdateInfo.NoChange;
    }
    
    // iteratively makes inferences based
    // on all the rules in this model.
    public UpdateInfo MakeInferences() {
        bool hasChanged = false;
        bool wasChanged = false;

        do {
            hasChanged = false;
            foreach (Rule r in sentenceRules) {
                UpdateInfo info = MakeInference(r);

                if (info == UpdateInfo.Warning) {
                    // TODO: do inconsistency stuff!
                    return UpdateInfo.Warning;
                }

                if (info == UpdateInfo.Updated) {
                    hasChanged = true;
                    wasChanged = true;
                }
            }
        } while (hasChanged);

        if (wasChanged) {
            return UpdateInfo.Updated;
        } else {
            return UpdateInfo.NoChange;            
        }
    }

    // Super compatible
    private HashSet<int> GetDomain(ISemanticType t) {
        Model currentModel = super;
        if (t.GetType() == typeof(FType)) {
            FType fType = (FType) t;
            LogicalForm formula = fType.GetFormula();
            int varID = formula.GetFreeVariables().Single<Variable>().GetID();
            
            ISemanticType baseType = fType.GetBaseType();
            
            // populating the set with all semantic values
            // (including those in super models)
            // possible error: basetype is undefined in M
            Dictionary<int,ISemanticValue>.KeyCollection baseSet = model[baseType].Keys;

            while (currentModel != null) {
                // possible error: basetype is undefined in M
                baseSet.Union<int>(currentModel.model[baseType].Keys);
            }
            
            HashSet<int> finalSet = new HashSet<int>();
            
            foreach (int i in baseSet) {
                if (Satisfies(formula.Bind(varID, new Constant(baseType, i)))) {
                    finalSet.Add(i);
                }
            }
            return finalSet;
        }
        
        HashSet<int> theSet = new HashSet<int>();
        
        foreach (int i in model[t].Keys) {
            theSet.Add(i);
        }

        while (currentModel != null) {
            // possible error: t is undefined in M
            theSet.UnionWith(currentModel.model[t].Keys);
        }

        return theSet;
    }

    // updates THIS model according to m (m is unchanged)
    // public bool update(Model m)
    // {
    //     bool hasUpdated = false;
    //     foreach (Entry<Integer, ISemanticValue> x in m.model.entrySet())
    //     {
    //         if (this.model.containsKey(x.getKey()))
    //         {
    //             hasUpdated = this.model.get(x.getKey()).update(x.getValue()) || hasUpdated;
    //         }
    //         else
    //         {
    //             this.model.put(x.getKey(), x.getValue().sClone());
    //             hasUpdated = true;
    //         }
    //     }
    //     return hasUpdated;
    // }
}