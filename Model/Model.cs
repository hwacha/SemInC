using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Model : ISemanticValue {
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
    private Dictionary<LogicalForm, Tuple<HashSet<Rule>, HashSet<Rule>>> activeRules;

    // the model which this model inherits from.
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
        this.activeRules = new Dictionary<LogicalForm, Tuple<HashSet<Rule>, HashSet<Rule>>>();
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
    // returns true if there was a change to the model,
    // false otherwise
    public bool MakeInference(Rule r) {
        if (!r.IsClosed()) {
            return false;
        }

        LogicalForm inference = r.GetInference(this);

        if (inference != null) {
            sentenceRules.Remove(r);

            if (!activeRules.ContainsKey(inference)) {
                activeRules[inference] =
                    Tuple.Create(new HashSet<Rule>(),
                                 new HashSet<Rule>());
            }

            activeRules[inference].Item2.Add(r);

            UpdateInfo info = MakeTrue(inference);

            // what was inferred was already true. Boring!
            if (info == UpdateInfo.NoChange) {
                return false;
            }

            // we've inferred something which turned out
            // to be inconsistent. We need to resolve it!
            if (info == UpdateInfo.Warning) {
                ResolveInconsistency(inference);
            }

            // we've either resolved an inconsistency or
            // inferred something without a hitch, so the
            // model has changed.
            return true;
            
        }

        return false;
    }
    
    // iteratively makes inferences based
    // on all the rules in this model.
    // returns true if the model changed at all,
    // false otherwise
    public bool MakeInferences() {
        bool hasChanged = false;
        bool wasChanged = false;

        do {
            hasChanged = false;

            Model currentModel = this;

            while (currentModel != null) {
                foreach (Rule r in sentenceRules) {
                    if (MakeInference(r)) {
                        hasChanged = true;
                        wasChanged = true;
                    }
                }
                currentModel = currentModel.super;
            }
        } while (hasChanged);

        return wasChanged;
    }

    private Policy BestAddition(LogicalForm l) {
        // TODO make this involve inferences too
        return new Add(l);
    }

    private Policy BestRemoval(LogicalForm l) {
        // first, we find what supports l.
        if (!activeRules.ContainsKey(l)) {
            // this means l was not inferred.
            // What should we do here?
            return new Remove(l);
        }
        
        foreach (Rule r in activeRules[l].Item2) {
            HashSet<LogicalForm> top = r.GetTop();
            HashSet<LogicalForm>[] bot = r.GetBottom();    

            // all the rules where l is on bottom
            // we can either side-step or remove a top one
            Policy bestRemoval = null;
            Policy bestShift = null;
            
            if(top.Count > 0) {
                bestRemoval = BestRemoval(top.First());

                // determine best top to remove
                foreach (LogicalForm parent in top) {
                    Policy contender = BestRemoval(parent);
                    if (CompareLikelihood(bestRemoval, contender) < 0) {
                        bestRemoval = contender;
                    }
                }
            }

            // THIS IS ALL THE SIDE-STEP STUFF
            for (int i = 0; i < bot.Length; i++) {
                HashSet<LogicalForm> currentTier = bot[i];

                foreach (LogicalForm sibling in currentTier) {
                    if (!Satisfies(sibling.Negate())) {
                        if (bestShift == null) {
                            bestShift = BestAddition(sibling);
                        } else {
                            int comparitor = CompareLikelihood(bestShift, BestAddition(sibling));
                            if (comparitor < 0) {
                                bestShift = BestAddition(sibling);
                            }
                        }
                    }
                }

                if (bestShift != null) {
                    break;
                }
            }

            // bestRemoval = bestRemoval.Concat(new Remove(l));

            bestShift = bestRemoval.GetDual().Concat(bestShift);

            Policy winner = (CompareLikelihood(bestRemoval, bestShift) > 0) ? bestRemoval : bestShift;

            return winner.Concat(new Remove(l));
        }

        return null;
        // TODO collect best results from different rules
    }

    // if there is an inconsistency with l and -l,
    // then we need to decide between them.
    // l is the new thing.
    private Policy ResolveInconsistency(LogicalForm l) {
        LogicalForm notL = l.Negate();
            
        // okay. new plan:
        // 1. have two helper methods:
        //    BestRemoval() and BestInsertion()
        // 2. recursively call BestRemoval() and
        //    BestInsertion() and conjoin sentences
        //    along best removal path
        // 3. (when there's a negation, check to see)
        //    whether, in the current rule, there's A
        //    on top or ~A on bottom
        // 4. problem for BestInsertion(): have to
        //    make copy of model to make hypothetical
        //    inferences
        // 5. ResolveInconsistency calls BestRemoval()
        //    on P and ~P, and then removes whichever
        //    wins out.
        // 6. Question simply becomes what to yield
        //    as output for BestInsertion() and BestRemoval()
        
        Policy bestL = BestRemoval(l);
        Policy bestNotL = BestRemoval(notL);

        int comparison = CompareLikelihood(BestRemoval(l), BestRemoval(notL));

        if (comparison > 0) {
            // TODO perform the removal strategy encoded by bestL
            return bestL;
        }

        if (comparison < 0) {
            // TODO perform the removal strategy encoded by bestNotL
            return bestNotL;
        }

        // TODO pick arbitrary one???
        // rn we're conservative - we pick the older one
        return bestNotL;
    }

    // returns 1 if Pr(a) > Pr(b),
    // 0 if Pr(a) = Pr(b)
    // -1 if Pr(a) < Pr(b)
    public int CompareLikelihood(Policy a, Policy b) {
        if (a == null && b == null) {
            return 0;
        }

        if (b == null) {
            return 1;
        }

        if (a == null) {
            return -1;
        }

        // TODO the actual comparison
        return 0;
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

    public int GetID() {
        return -1; // TODO idk
    }

    // public ISemanticValue SClone() {
    //     Model newModel = new Model(this.super);

    //     foreach (ISemanticType t in this.model.Keys) {
    //         foreach (int i in this.model[t].Keys) {
    //             newModel.Add(t, i, this.model[t][i]);
    //         }
    //     }

    //     foreach (ISemanticType t in this.formulaRules.Keys) {
    //         newModel.formulaRules[t] = new HashSet<Rule>();
    //         foreach (Rule r in this.formulaRules[t]) {
    //             newModel.formulaRules[t].Add(r);
    //         }
    //     }

    //     foreach (Rule r in this.sentenceRules) {
    //         newModel.sentenceRules.Add(r);
    //     }

    //     foreach (Rule r in this.activeRules) {
    //         newModel.activeRules.Add(r);
    //     }

    //     return newModel;
    // }

    // // updates THIS model according to m (m is unchanged)
    // public bool Update(ISemanticValue that) {

    //     if (!(that.GetType() == typeof(Model))) {
    //         return false;
    //     }

    //     Model m = (Model) that;

    //     bool hasUpdated = false;

    //     // go through all of the entries in the model,
    //     // and replace all the old stuff (this model)
    //     // with the new stuff (found in the input model)
    //     // ----------
    //     // foreach (int i in m.model.EntrySet()) {
    //     //     if (this.model.containsKey(x.getKey())) {
    //     //         hasUpdated = this.model.get(x.GetKey()).update(x.GetValue()) || hasUpdated;
    //     //     }
    //     //     else {
    //     //         this.model.put(x.getKey(), x.GetValue().SClone());
    //     //         hasUpdated = true;
    //     //     }
    //     // }
    //     return hasUpdated;
    // }
}