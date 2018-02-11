using System.Collections;
using System.Collections.Generic;

public class Or : LogicalForm {
    private LogicalForm left;
    private LogicalForm right;

    public Or(LogicalForm left, LogicalForm right) : base(new T()) {
        if (!(left.IsFormula() && right.IsFormula())) {
            // error!
            return;
        }
        this.left = left;
        this.right = right;
        this.isFormula = true;
        this.freeVariables = left.MergeVariables(right);
    }

    public override UpdateInfo Make(Model m, TruthValue.T v) {
        Function wrappers = (Function) m.Get(Model.WRAPPERS_ID);
        Wrapper w = new Wrapper(this);
        
        // if this logical form is already in the model
        if (wrappers.HasInput(w)) {
            TruthValue oldValue = (TruthValue) wrappers.Apply(w);
            if (oldValue.Get() == v) {
                return UpdateInfo.NoChange;
            }

            if (oldValue.Get() == TruthValue.T.Unknown) {
                return oldValue.Add(v);
            }
            
            return UpdateInfo.Warning;
        }

        // if this logical form isn't defined yet
        wrappers.Set(w, new TruthValue(v));
        return UpdateInfo.Updated;
    }

    public override ISemanticValue Denotation(Model m) {
        return ((Function) m.Get(Model.WRAPPERS_ID)).Apply(new Wrapper(this));
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new Or(left.Bind(id, l), right.Bind(id, l));
    }
}