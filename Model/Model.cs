using System.Collections;
using System.Collections.Generic;

public class Model { //this has only been partially changed from Java to C#
    private int ID_COUNT = 1000;
    private Dictionary<int, ISemanticValue> model = new Dictionary<int, ISemanticValue>();
    // // to speed things up, change from a map to semantic values to rules
    private HashSet<Rule> rules = new HashSet<Rule>();

    public static int WRAPPERS_ID = 5;

    public int GetNextAvailableID() {
        ID_COUNT++;
        return ID_COUNT;
    }

    // public Model() { }
    // 
    public UpdateInfo Add(int id, ISemanticValue v) {
        if (model.ContainsKey(id)) {
            return UpdateInfo.NoChange;
        }
        model[id] = v;
        return UpdateInfo.Updated;
    }

    // public bool Add(ISemanticValue[] values)
    // {
    //     foreach (ISemanticValue x in values)
    //     {
    //         model[x.getid()] = x;
    //     }
    //     return true; //TODO make more restrictive
    // }

    // public bool Add(Rule[] rs)
    // {
    //     foreach (Rule r in rs)
    //     {
    //         rules.Add(r);
    //     }
    //     return true; //TODO make more restrictive
    // }

    public ISemanticValue Get(int id) {
        return model[id];
    }

    // public bool Satisfies(proof.LogicalForm l)
    // {
    //     if (l.isClosed() && l.isFormula())
    //     {
    //         ISemanticValue s = l.denotation(this);
    //         //that.GetType() == typeof(Function))
    //         if (s.GetType() == typeof(TruthValue))
    //         {
    //             TruthValue t = (TruthValue)s;
    //             return t.isTrue();
    //         }
    //     }
    //     return false;
    // }
    
    // Updates the Model such that l has the truth value v
    public UpdateInfo Make(LogicalForm l, TruthValue.T v) {
        if (l.IsClosed() && l.IsFormula()) {
            return UpdateInfo.NoChange;
        }

        return l.Make(this, v);
    }

    // public bool Update(LogicalForm l) //possibly rename bc Update means something else in Unity
    // {
    //     if (l.isClosed() && l.isFormula())
    //     {
    //         ISemanticValue s = l.denotation(this);
    //         if (s.GetType() == typeof(TruthValue))
    //         {
    //             TruthValue t = (TruthValue)s;
    //             bool isNot = (l.getType() == typeof(Not));
    //             if (isNot)
    //             {
    //                 Not n = (Not)l;
    //                 t = (TruthValue)n.sub().denotation(this);
    //             }
    //             bool changed = t.Add(!isNot); //TODO this is hacky and won't work for double negatives
    //             return changed;
    //         }
    //     }
    //     return false;
    // }

    // // updates THIS model according to m (m is unchanged)
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

    // private Set<Integer> getDomain(SemanticType t)
    // {
    //     if (!t.equals(new proof.E()))
    //     {
    //         return null; //TODO fuck my model isn't good enough. I'll have to type the values
    //     }

    //     HashSet<Integer> keys = new HashSet<Integer>();

    //     foreach (int i in model.keySet())
    //     {
    //         if (model.get(i).getType() == typeof(Individual)) {
    //             keys.Add(i);
    //         }
    //     }
    //     return keys;
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
    //         foreach (Rule r in rules)
    //         {
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