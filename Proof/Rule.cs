using System.Collections;
using System.Collections.Generic;

public class Rule {
    // 1. store formula rule
    // 2. when rule is added to a model, make all the sentence rules you can
    // 3. if a semantic value gets added, check the rules to add another sentence rule
    // 4. formula rules field (which has a set of semantic values)
    // 5. sentence rules field

    private HashSet<LogicalForm> top;
    private HashSet<LogicalForm>[] bot;
    private HashSet<Variable> freeVariables;

    public Rule(HashSet<LogicalForm> top, HashSet<LogicalForm>[] bot) {
        this.top = top;
        this.bot = bot;

        freeVariables = new HashSet<Variable>();

        // getting all the free variables in all of the logical forms
        foreach (LogicalForm l in top) {
            foreach (Variable v in l.GetFreeVariables()) {
               freeVariables.Add(v);
            }
        }

        for (int i = 0; i < bot.Length; i++) {
            foreach (LogicalForm l in bot[i]) {
                foreach (Variable v in l.GetFreeVariables()) {
                    freeVariables.Add(v);
                }
            }    
        }
    }

    public HashSet<Variable> GetFreeVariables() {
        return freeVariables;
    }

    public bool IsClosed() {
        return freeVariables.Count == 0;
    }

    // returns null if nothing should be inferred,
    // otherwise, return what should be inferred
    // when this rule is applied to the model
    // this should only be called when there are no free variables.
    public LogicalForm GetInference(Model m) {
        LogicalForm uniquelyUnsatisfiedTop = null;
        foreach (LogicalForm l in top) {
            if (!m.Satisfies(l)) {
                if (uniquelyUnsatisfiedTop == null) {
                    uniquelyUnsatisfiedTop = l;
                } else {
                    // this is the case where more than
                    // one top sentence is false
                    return null;
                }
            }
        }
        
        // is there one and only one top sentence which
        // m fails to satisfy? If all the bottom sentences
        // are rejected, then reject that unique sentence.
        if (uniquelyUnsatisfiedTop == null) {
            for (int i = 0; i < bot.Length; i++) {
                foreach (LogicalForm l in bot[i]) {
                    if (!m.Satisfies(new Not(l))) {
                        // this means the rule should
                        // not infer anything at this point
                        return null;
                    }
                }
            }
            // if we ever reach this point,
            // it must be that all bottom sentences
            // were rejected. So we should reject
            // the only top sentence not satisfied.
            return new Not(uniquelyUnsatisfiedTop);
        } else {
            // this is the case where all sentences
            // on the top were true,
            // so we should infer something on the bottom.
            for (int i = 0; i < bot.Length; i++) {
                // first, we go through and see if
                // any sentence on the bottom is true already
                // if so, we infer nothing new (but still return
                // that, to let the system know this rule is active).
                foreach (LogicalForm l in bot[i]) {
                    if (m.Satisfies(l)) {
                        return l;
                    }
                }
            }
            // if none of the bottom sentences are satisfied by M,
            // then infer the most likely one, via the "tiers"
            for (int i = 0; i < bot.Length; i++) {
                LogicalForm uniquelySatisfiable = null;
                foreach (LogicalForm l in bot[i]) {
                    if (!m.Satisfies(new Not(l))) {
                        if (uniquelySatisfiable == null) {
                            uniquelySatisfiable = l;
                        } else {
                            // for now, we treat it as though
                            // there's no change, when more than
                            // (We might want to change it
                            // so that there's a disjunct)
                            return null;
                        }
                    }
                }
                // this means one and only one sentence in this tier
                // is satisfiable. So, we infer it!
                if (uniquelySatisfiable != null) {
                    return uniquelySatisfiable;
                }
            }
            // this means that all of the top sentences were true,
            // but all of the bottom sentences were false.
            // this should make the system try to resolve an
            // inconsistency; or perhaps this case should never happen.
            // for now, I'll just return null.
            return null;
        }
    }

    public Rule Bind(int id, LogicalForm replace) {

        HashSet<LogicalForm> newTop = new HashSet<LogicalForm>();
        HashSet<LogicalForm>[] newBot = new HashSet<LogicalForm>[bot.Length];

        foreach (LogicalForm l in top) {
            newTop.Add(l.Bind(id, replace));
        }
        for (int i = 0; i < bot.Length; i++) {
            foreach (LogicalForm l in bot[i]) {
                newBot[i].Add(l.Bind(id, replace));
            }
        }

        return new Rule(newTop, newBot);
    }

    // public HashSet<LogicalForm> GetTop() {
    //     return top;
    // }

    // public List<LogicalForm> GetBottom() {
    //     return bot;
    // }
    
    // public Rule(HashSet<LogicalForm> top) {

    // }

    // public bool AddTop(LogicalForm l) {
    //     if (l.IsFormula()) {
    //         top.Add(l);
    //         return true;
    //     }
    //     return false;
    // }

    // public bool AddBottom(LogicalForm l) {
    //     if (l.IsFormula()) {
    //         bot.Add(l);
    //         return true;
    //     }
    //     return false;
    // }

    // private bool Move(LogicalForm l, List<LogicalForm> from, List<LogicalForm> to) {
    //     if (from.Contains(l)) {
    //         from.Remove(l);
    //         to.Add(l);
    //         return true;
    //     }
    //     return false;
    // }

    // public bool MoveUp(LogicalForm l) {
    //     return Move(l, bot, top);
    // }

    // public bool MoveDown(LogicalForm l) {
    //     return Move(l, top, bot);
    // }

    //    public Rule ToContradiction()
    //    {
    //        Rule nr = clone();
    //        for (LogicalForm l : bot)
    //        {
    //            nr.moveUp(l);
    //        }
    //        return nr;
    //    }

    //    public Rule ToTautology()
    //    {
    //        Rule nr = clone();
    //        for (LogicalForm l : bot)
    //        {
    //            nr.moveDown(l);
    //        }
    //        return nr;
    //    }

    //    public List<Rule> GetCanonicalRules()
    //    {
    //        Rule contradiction = toContradiction();
    //        LinkedList<Rule> canonicalRules = new LinkedList<Rule>();
    //        for (LogicalForm l : contradiction.top)
    //        {
    //            Rule nr = contradiction.clone();
    //            nr.moveDown(l);
    //            canonicalRules.add(nr);
    //        }
    //        return canonicalRules;
    //    }

    //    public Rule Clone()
    //    {
    //        Rule nr = new Rule();
    //        for (LogicalForm l : top)
    //        {
    //            nr.addTop(l);
    //        }
    //        for (LogicalForm l : bot)
    //        {
    //            nr.addBottom(l);
    //        }
    //        return nr;
    //    }

    //    public String toString()
    //    {
    //        StringBuilder s = new StringBuilder();

    //        for (LogicalForm l : top)
    //        {
    //            s.append(l);
    //            s.append(",");
    //        }
    //        s.append("\u22A4 \u22A2 ");

    //        for (LogicalForm l : bot)
    //        {
    //            s.append(l);
    //            s.append(",");
    //        }

    //        s.append("\u22A5");

    //        return s.toString();
    //    }
    //}

}
