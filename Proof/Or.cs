using System;
using System.Collections;
using System.Collections.Generic;

public class Or : LogicalForm {
    private HashSet<LogicalForm> subs;

    public Or(HashSet<LogicalForm> subs) : base(new T()) {
        if (subs.Count == 0) {
            throw new ArgumentException();
        }

        this.freeVariables = new HashSet<Variable>();
        foreach (LogicalForm l in subs) {
            if (!l.IsFormula()) {
                throw new ArgumentException();
            }
            this.freeVariables = l.MergeVariables(this.freeVariables);
        }
        this.subs = subs;
        this.isFormula = true;
    }

    public override UpdateInfo Make(Model m, TruthValue.T v) {
        Function wrappers = (Function) m.Get(GetSemanticType(), Model.WRAPPERS_ID);
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
        return ((Function) m.Get(GetSemanticType(), Model.WRAPPERS_ID)).Apply(new Wrapper(this));
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        HashSet<LogicalForm> newSubs = new HashSet<LogicalForm>();
        foreach (LogicalForm sub in this.subs) {
            newSubs.Add(sub.Bind(id, l));
        }

        return new Or(newSubs);
    }
}