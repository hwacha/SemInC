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

	// abstract ISemanticValue Denotation(Model m);

    public abstract LogicalForm Bind(int id, LogicalForm l);

    
}
