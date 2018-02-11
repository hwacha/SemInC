using System.Collections;
using System.Collections.Generic;

public class Lambda : LogicalForm {
    private Variable v;
    private LogicalForm l;

    public Lambda(Variable v, LogicalForm l)
        : base(new Arrow(v.GetSemanticType(), l.GetSemanticType())) {
        this.v = v;
        this.l = l;

        this.freeVariables = l.CloneVariables();
        freeVariables.Remove(v);
    }

    public LogicalForm Apply(LogicalForm x) {
        return l.Bind(v.GetID(), x);
    }

    public override LogicalForm Bind(int id, LogicalForm lf) {
        if (id == v.GetID()) {
            return this;
        }
        return new Lambda(v, l.Bind(id, lf));
    }

    public override ISemanticValue Denotation(Model m) {
        return null; //TODO this is also wrong.
        // Should I try to detect when such a
        // function is in the model, or just spawn a new one?
    }

    public override string ToString() {
        return "\u03BB" + v + "[" + l + "]";
    }
}
