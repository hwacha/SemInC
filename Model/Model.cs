using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Model {
    private int ID_COUNT = 1000;
    private Dictionary<ISemanticType, Dictionary<int, ISemanticValue>> model;

    // the fields for rules
    private Dictionary<ISemanticType, HashSet<Rule>> formulaRules;
    private HashSet<Rule> sentenceRules;
    private HashSet<Rule> activeRules;

    Model super;
    // what needs to change to support model inheretance:d
    // 1. Denotation: if not defined in a lower model, should look higher
    // 2. Satisfies: if not in lower model (for P and -P), then look higher
    // 3. Make/Update: only affect lowest level called

    public static int WRAPPERS_ID = 5;

    public Model(Model super) {
        this.model = new Dictionary<ISemanticType, Dictionary<int, ISemanticValue>>();
        this.formulaRules = new Dictionary<ISemanticType, HashSet<Rule>>();
        this.sentenceRules = new HashSet<Rule>();
        this.activeRules = new HashSet<Rule>();
        this.super = super;
    }

    public Model() : this(null) {}

    public int GetCurrentID() {
        return ID_COUNT;
    }

    public int GetNextAvailableID() {
        ID_COUNT++;
        return ID_COUNT;
    }

    public Model GetSuperModel() {
        return super;
    }

    public UpdateInfo Add(ISemanticType t, int id, ISemanticValue v) {
        if (!model.ContainsKey(t)) {
            model[t] = new Dictionary<int, ISemanticValue>();
        }
        Dictionary<int, ISemanticValue> modelByType = model[t];
        if (modelByType.ContainsKey(id)) {
            return UpdateInfo.NoChange;
        }
        modelByType[id] = v;

        if (formulaRules.ContainsKey(t)) {
            foreach (Rule fr in formulaRules[t]) {
                HashSet<Rule> srs = GetSentenceRules(fr);
                foreach (Rule sr in srs) {
                    sentenceRules.Add(sr);
                }
            }
        }

        return UpdateInfo.Updated;
    }

    // generates all the sentence-rules from a formula-rule
    public HashSet<Rule> GetSentenceRules(Rule r) {
        HashSet<Rule> sentenceRules = new HashSet<Rule>();

        if (r.IsClosed()) {
            sentenceRules.Add(r);
            return sentenceRules;
        }

        HashSet<Variable> freeVariables = r.GetFreeVariables();

        foreach (Variable v in freeVariables) {
            HashSet<int> ids = GetDomain(v.GetSemanticType());
            foreach (int id in ids) {
                Rule boundRule = r.Bind(v.GetID(), new Constant(v.GetSemanticType(), id));
                sentenceRules.UnionWith(GetSentenceRules(boundRule));
            }
        }

        return sentenceRules;
    }

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