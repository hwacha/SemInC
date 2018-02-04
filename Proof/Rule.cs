using System.Collections;
using System.Collections.Generic;

public class Rule {

    private List<LogicalForm> top = new List<LogicalForm>();
    private List<LogicalForm> bot = new List<LogicalForm>();

    public List<LogicalForm> GetTop() {
        return top;
    }

    public List<LogicalForm> GetBottom() {
        return bot;
    }

    public bool AddTop(LogicalForm l) {
        if (l.IsFormula()) {
            top.Add(l);
            return true;
        }
        return false;
    }

    public bool AddBottom(LogicalForm l) {
        if (l.IsFormula()) {
            bot.Add(l);
            return true;
        }
        return false;
    }

    private bool Move(LogicalForm l, List<LogicalForm> from, List<LogicalForm> to) {
        if (from.Contains(l)) {
            from.Remove(l);
            to.Add(l);
            return true;
        }
        return false;
    }

    public bool MoveUp(LogicalForm l) {
        return Move(l, bot, top);
    }

    public bool moveDown(LogicalForm l) {
        return Move(l, top, bot);
    }

    //    public Rule ToContradiction()
    //    {
    //        Rule nr = clone();
    //        for (LogicalForm l : bot)
    //        {
    //            nr.moveUp(l);
    //        }
    //        return nr;
    //    }

    //    public Rule toTautology()
    //    {
    //        Rule nr = clone();
    //        for (LogicalForm l : bot)
    //        {
    //            nr.moveDown(l);
    //        }
    //        return nr;
    //    }

    //    public List<Rule> getCanonicalRules()
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

    //    public Set<Variable> getFreeVariables()
    //    {
    //        HashSet<Variable> vars = new HashSet<Variable>();
    //        for (LogicalForm l : top)
    //        {
    //            vars.addAll(l.getFreeVariables(new HashSet<Variable>()));
    //        }
    //        for (LogicalForm l : bot)
    //        {
    //            vars.addAll(l.getFreeVariables(new HashSet<Variable>()));
    //        }
    //        return vars;
    //    }

    //    public Rule Bind(int id, LogicalForm replace)
    //    {
    //        Rule newRule = new Rule();
    //        for (LogicalForm l : top)
    //        {
    //            newRule.addTop(l.bind(id, replace));
    //        }
    //        for (LogicalForm l : bot)
    //        {
    //            newRule.addBottom(l.bind(id, replace));
    //        }

    //        return newRule;

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
