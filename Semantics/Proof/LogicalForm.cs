using System.Collections;
using System.Collections.Generic;

public abstract class LogicalForm {
    protected ISemanticType type;
    protected bool isFormula;
    protected HashSet<Variable> freeVariables = null;

    bool IsFormula() {
        return isFormula;
    }
    bool IsClosed() {
        return freeVariables == null || freeVariables.Count == 0;
    }

    public ISemanticType GetSemanticType() {
        return type;
    }
	// abstract ISemanticValue Denotation(Model m);

    public abstract LogicalForm Bind(int id, LogicalForm l);

    HashSet<Variable> GetFreeVariables() {
        return freeVariables;
    }
}
