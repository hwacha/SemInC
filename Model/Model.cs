using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Model {
    private int ID_COUNT = 1000;
    private Dictionary<ISemanticType, Dictionary<int, ISemanticValue>> model;
    // to speed things up, change from a map to semantic values to rules
    private HashSet<Rule> rules = new HashSet<Rule>();

    Model super;
    // what needs to change to support model inheretance:
    // 1. Denotation: if not defined in a lower model, should look higher
    // 2. Satisfies: if not in lower model (for P and -P), then look higher
    // 3. Make/Update: only affect lowest level called

    public static int WRAPPERS_ID = 5;

    public Model() {
        this.model = new Dictionary<ISemanticType, Dictionary<int, ISemanticValue>>();
        this.super = null;
    }

    public Model(Model super) {
        this.model = new Dictionary<ISemanticType, Dictionary<int, ISemanticValue>>();
        this.super = super;
    }

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
        return UpdateInfo.Updated;
    }

    public UpdateInfo Add(Rule r) {
        if (rules.Contains(r)) {
            return UpdateInfo.NoChange;
        }
        rules.Add(r);
        return UpdateInfo.Updated;
    }

    public ISemanticValue Get(ISemanticType t, int id) {
        return model[t][id];
    }
    
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
    
    // Updates the Model such that l has the truth value v
    private UpdateInfo Make(LogicalForm l, TruthValue.T v) {
        if (l.IsClosed() && l.IsFormula()) {
            return UpdateInfo.NoChange;
        }
        return l.Make(this, v);
    }

    //possibly rename bc Update means something else in Unity
    public UpdateInfo Update(LogicalForm s) {
        return Make(s, TruthValue.T.True);
    }

    private HashSet<int> GetDomain(ISemanticType t) {
        if (t.GetType() == typeof(FType)) {
            FType fType = (FType) t;
            LogicalForm formula = fType.GetFormula();
            int varID = formula.GetFreeVariables().Single<Variable>().GetID();
            
            ISemanticType baseType = fType.GetBaseType();
            Dictionary<int,ISemanticValue>.KeyCollection baseSet = model[baseType].Keys;
            
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

        return theSet;
    }

    //     // // updates THIS model according to m (m is unchanged)
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

    // // updates according to a rule, assuming variables have been bound with a value
    // private bool UpdateSentence(Rule r) {
    //     foreach (LogicalForm l in r.getTop())
    //     {
    //         if (!this.satisfies(l))
    //         {
    //             return false;
    //         }
    //     }
    //     List<LogicalForm> bot = r.getBottom();
    //     if (bot.size() == 1)
    //     {
    //         return this.update(bot.get(0));
    //     }
    //     return false;
    // }

    // public bool Update(Rule r)
    // {
    //     Set<Variable> vars = r.getFreeVariables();
    //     HashSet<Rule> rs = new HashSet<Rule>();
    //     rs.Add(r);
    //     foreach (Variable v in vars)
    //     {
    //         int varID = v.getID();
    //         HashSet<Rule> newRules = new HashSet<Rule>();
    //         foreach (Rule x in rs)
    //         {
    //             foreach (int i in getDomain(v.getType()))
    //             {
    //                 newRules.add(x.bind(varID, new Constant(v.getType(), i)));
    //             }
    //         }
    //         rs = newRules;
    //     }
    //     bool anyChange = false;
    //     foreach (Rule filledRs in rs)
    //     {
    //         foreach (Rule canonicalR in filledRs.getCanonicalRules())
    //         {
    //             anyChange = (UpdateSentence(canonicalR) || anyChange);
    //         }
    //     }
    //     return anyChange;
    // }

    // public void Update() //possibly rename bc Update is something else in unity
    // {
    //     bool didUpdate;
    //     do
    //     {
    //         didUpdate = false;
    //         foreach (Rule r in rules) {
    //             didUpdate = (Update(r) || didUpdate);
    //         }
    //     } while (didUpdate);
    // }

    // private bool InconsistencyHelper(SemanticValue v)
    // {
    //     if (v.getType() == typeof(TruthValue))
    //     {
    //         if (((TruthValue)v).isBoth())
    //         {
    //             return true;
    //         }
    //     }
    //     if (v.getType() == typeof(Function))
    //     {
    //         foreach (SemanticValue x in ((Function)v).codomain())
    //         {
    //             if (InconsistencyHelper(x))
    //             {
    //                 return true;
    //             }
    //         }
    //     }
    //     return false;
    // }

    // public bool HasInconsistency()
    // {
    //     foreach (SemanticValue v in model.values())
    //     {
    //         if (InconsistencyHelper(v))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

}