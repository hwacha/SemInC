using System.Collections;
using System.Collections.Generic;

public abstract class LogicalForm {
    protected ISemanticType type;
    protected bool isFormula;
    protected HashSet<Variable> freeVariables = null;

    protected LogicalForm(ISemanticType type) {
        this.type = type;
    }

    public ISemanticType GetSemanticType() {
        return type;
    }

    public bool IsFormula() {
        return isFormula;
    }

    public bool IsClosed() {
        return freeVariables == null || freeVariables.Count == 0;
    }

    public HashSet<Variable> GetFreeVariables() {
        return freeVariables;
    }

	public abstract ISemanticValue Denotation(Model m);

    public virtual UpdateInfo Make(Model m, TruthValue.T v) {
        if (!(IsFormula() && IsClosed())) {
            return UpdateInfo.NoChange;
        }

        TruthValue currentValue = (TruthValue) Denotation(m);

        if (currentValue.Get() == v) {
            return UpdateInfo.NoChange;
        }

        if (currentValue.Get() == TruthValue.T.Unknown) {
            return currentValue.Add(v);
        }

        return UpdateInfo.Warning;
    }

    public HashSet<Variable> CloneVariables() {
        HashSet<Variable> newVs = new HashSet<Variable>();

        foreach (Variable v in freeVariables) {
            newVs.Add(v);
        }

        return newVs;
    }

    public HashSet<Variable> MergeVariables(LogicalForm l) {

        HashSet<Variable> newVs = new HashSet<Variable>();

        if (freeVariables != null) {
            foreach (Variable v in freeVariables) {
                newVs.Add(v);
            }
        }

        if (l.GetFreeVariables() != null) {
            foreach (Variable v in l.GetFreeVariables()) {
                newVs.Add(v);
            }
        }

        return newVs;
    }

    public virtual LogicalForm Negate() {
        return new Not(this);
    }

    public abstract LogicalForm Bind(int id, LogicalForm l);
}
