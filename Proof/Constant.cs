using System.Collections;
using System.Collections.Generic;

public class Constant : LogicalForm {
    private int id;

	public Constant(ISemanticType type, int id) : base(type) {
		this.type = type;
		this.id = id;
        this.isFormula = type.GetType() == typeof(T);
	}

    public override LogicalForm Bind(int id, LogicalForm l) {
        return this;
    }

    public override string ToString() {
        return "[" + id + "]";
    }


	public override ISemanticValue Denotation(Model m) {
		return m.Get(GetSemanticType(), id);
	}
}
