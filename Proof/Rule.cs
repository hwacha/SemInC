using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class Rule
{
    // 1. store formula rule
    // 2. when rule is added to a model, make all the sentence rules you can
    // 3. if a semantic value gets added, check the rules to add another sentence rule
    // 4. formula rules field (which has a set of semantic values)
    // 5. sentence rules field

    private HashSet<LogicalForm> top;
    private HashSet<LogicalForm>[] bot;
    private HashSet<Variable> freeVariables;

    public Rule(HashSet<LogicalForm> top, HashSet<LogicalForm>[] bot)
    {
        this.top = top;
        this.bot = bot;

        freeVariables = new HashSet<Variable>();

        // getting all the free variables in all of the logical forms
        foreach (LogicalForm l in top)
        {
            foreach (Variable v in l.GetFreeVariables())
            {
                freeVariables.Add(v);
            }
        }

        for (int i = 0; i < bot.Length; i++)
        {
            foreach (LogicalForm l in bot[i])
            {
                foreach (Variable v in l.GetFreeVariables())
                {
                    freeVariables.Add(v);
                }
            }
        }
    }

    public HashSet<Variable> GetFreeVariables()
    {
        return freeVariables;
    }

    public bool IsClosed()
    {
        return freeVariables.Count == 0;
    }

    // returns null if nothing should be inferred.
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
        if (uniquelyUnsatisfiedTop != null) {
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
            return uniquelyUnsatisfiedTop.Negate();
        } else {
            // this is the case where all sentences
            // on the top were true,
            // so we should infer something on the bottom.

            // infer the disjunction of satisfiable sentences
            // in the first tier
            HashSet<LogicalForm> satisfiableSentences = new HashSet<LogicalForm>();

            if (bot.Length == 0) {
                return null;
            }

            foreach (LogicalForm l in bot[0]) {
                if (!m.Satisfies(l.Negate())) {
                    satisfiableSentences.Add(l);
                }
            }
            // this means one and only one sentence in this tier
            // is satisfiable. So, we infer it!
            if (satisfiableSentences.Count == 1) {
                return satisfiableSentences.First();
            }

            if (satisfiableSentences.Count > 1) {
                return new Or(satisfiableSentences);
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

    public override string ToString() {
        StringBuilder s = new StringBuilder();

        foreach (LogicalForm l in top) {
            s.Append(l);
            s.Append(",");
        }

        s.Append("|-");

        for (int i = 0; i < bot.Length; i++) {
            foreach (LogicalForm l in bot[i]) {
                s.Append(l);
                s.Append(",");
            }
        }

        return s.ToString();
    }

}

// X says that P |- {X believes that P}, B
// X believes that P, F, C |- {P}, X was mistaken
// ~P

// Decide between B and C

// Bill says that it's raining
// it's not raining
