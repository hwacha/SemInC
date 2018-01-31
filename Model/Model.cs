using System.Collections;
using System.Collections.Generic;

public class Model { //this has only been partially changed from Java to C#

    //package model;
    //import proof.*;
    //import syntax.Expression;

    public static int ID_COUNT = -1;
    private Dictionary<int, ISemanticValue> model = new Dictionary<int, ISemanticValue>();
    // to speed things up, change from a map to semantic values to rules
    private HashSet<Rule> rules = new HashSet<Rule>();

    public Model() { }
}

//         public bool Add(ISemanticValue[] values)
//         {
//             foreach (ISemanticValue x in values)
//             {
//                 model[x.GetID()] = x;
//             }
//             return true; //TODO make more restrictive
//         }

//         public bool Add(Rule[] rs)
//         {
//             foreach (Rule r in rs)
//             {
//                 rules.Add(r);
//             }
//             return true; //TODO make more restrictive
//         }

//         public ISemanticValue get(int id)
//         {
//             return model.get(id);
//         }

//         public bool Satisfies(proof.LogicalForm l)
//         {
//             if (l.isClosed() && l.isFormula())
//             {
//                 ISemanticValue s = l.denotation(this);
//             //that.GetType() == typeof(Function))
//             if (s.GetType() == typeof(TruthValue)) {
//                     TruthValue t = (TruthValue)s;
//                     return t.isTrue();
//                 }
//             }
//             return false;
//         }

//         public bool Update(LogicalForm l) //possibly rename bc Update means something else in Unity
//         {
//             if (l.isClosed() && l.isFormula())
//             {
//                 ISemanticValue s = l.denotation(this);
//                 if (s.GetType() == typeof(TruthValue)) {
//                     TruthValue t = (TruthValue)s;
//                     bool isNot = l.getType() == typeof(Not);
//                     if (isNot)
//                     {
//                         Not n = (Not)l;
//                         t = (TruthValue)n.sub().denotation(this);
//                     }
//                     bool changed = t.Add(!isNot); //TODO this is hacky and won't work for double negatives
//                     return changed;
//                 }
//             }
//             return false;
//         }

//         // updates THIS model according to m (m is unchanged)
//         public bool update(Model m)
//         {
//             bool hasUpdated = false;
//             foreach (Entry<Integer, ISemanticValue> x in m.model.entrySet())
//             {
//                 if (this.model.containsKey(x.getKey()))
//                 {
//                     hasUpdated = this.model.get(x.getKey()).update(x.getValue()) || hasUpdated;
//                 }
//                 else
//                 {
//                     this.model.put(x.getKey(), x.getValue().sClone());
//                     hasUpdated = true;
//                 }
//             }
//             return hasUpdated;
//         }

//         private Set<Integer> getDomain(SemanticType t)
//         {
//             if (!t.equals(new proof.E()))
//             {
//                 return null; //TODO fuck my model isn't good enough. I'll have to type the values
//             }

//             HashSet<Integer> keys = new HashSet<Integer>();

//             foreach (int i in model.keySet())
//             {
//                 if (model.get(i) instanceof Individual) {
//                 keys.Add(i);
//             }
//         }
		
// 		return keys;
// 	}

//     // updates according to a rule, assuming
//     // variables have been bound with a value
//     private bool UpdateSentence(Rule r)
//     {
//         foreach (LogicalForm l in r.getTop())
//         {
//             if (!this.satisfies(l))
//             {
//                 return false;
//             }
//         }
//         List<LogicalForm> bot = r.getBottom();
//         if (bot.size() == 1)
//         {
//             return this.update(bot.get(0));
//         }
//         return false;
//     }

//     public bool update(Rule r)
//     {
//         Set<Variable> vars = r.getFreeVariables();
//         HashSet<Rule> rs = new HashSet<Rule>();
//         rs.Add(r);
//         foreach (Variable v in vars)
//         {
//             int varID = v.getID();
//             HashSet<Rule> newRules = new HashSet<Rule>();
//             for (Rule x : rs)
//             {
//                 for (int i : getDomain(v.getType()))
//                 {
//                     newRules.add(x.bind(varID, new Constant(v.getType(), i)));
//                 }
//             }
//             rs = newRules;
//         }
//         bool anyChange = false;
//         foreach (Rule filledRs in rs)
//         {
//             foreach (Rule canonicalR in filledRs.getCanonicalRules())
//             {
//                 anyChange = updateSentence(canonicalR) || anyChange;
//             }
//         }

//         return anyChange;
//     }

//     public void Update() //possibly rename bc update is something else in unity
//     {
//         bool didUpdate;
//         do
//         {
//             didUpdate = false;
//             foreach (Rule r in rules)
//             {
//                 didUpdate = update(r) || didUpdate;
//             }
//         } while (didUpdate);
//     }

//     private bool InconsistencyHelper(SemanticValue v)
//     {
//         if (v.getType() == typeof(TruthValue)) {
//             if (((TruthValue)v).isBoth())
//             {
//                 return true;
//             }
//         }
//         if (v.getType() == typeof(Function)) {
//             foreach (SemanticValue x in ((Function)v).codomain())
//             {
//                 if (inconsistencyHelper(x))
//                 {
//                     return true;
//                 }
//             }
//         }
//         return false;
//     }

//     public bool HasInconsistency()
//     {
//         foreach (SemanticValue v in model.values())
//         {
//             if (inconsistencyHelper(v))
//             {
//                 return true;
//             }
//         }
//         return false;
//     }
// }

// }
