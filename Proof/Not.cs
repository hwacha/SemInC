using System.Collections;
using System.Collections.Generic;

public class Not : LogicalForm {
    private LogicalForm sub;

    public Not(LogicalForm sub) : base(new T()) {
        if (!sub.IsFormula()) {
            // error!
            return;
        }
        this.sub = sub;
        this.isFormula = true;
        this.freeVariables = sub.GetFreeVariables();
    }

    private TruthValue.T Negate(TruthValue.T v) {
        if (v == TruthValue.T.True) {
            return TruthValue.T.False;
        }

        if (v == TruthValue.T.False) {
            return TruthValue.T.True;
        }

        return TruthValue.T.Unknown;
    }

    public override UpdateInfo Make(Model m, TruthValue.T v) {
        return sub.Make(m, Negate(v));
    }

    public override ISemanticValue Denotation(Model m) {
        ISemanticValue subDenotation = sub.Denotation(m);
        return new TruthValue(Negate(((TruthValue) subDenotation).Get()));
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new Not(sub.Bind(id, l));
    }
}
