using System;
using System.Collections.Generic;

public class Variable : LogicalForm {
	private int id;

	public Variable(ISemanticType type, int id) : base(type) {
		this.id = id;
		this.isFormula = type.GetType() == typeof(T);

		this.freeVariables = new HashSet<Variable>();
		this.freeVariables.Add(this);
	}

	public int GetID() {
		return id;
	}

	public override ISemanticValue Denotation(Model m) {
		return null;
	}

	public override String ToString() {
		return "{" + id + "}";
	}

	public override bool Equals(Object o) {
		if (o.GetType() == typeof(Variable)) {
			Variable that = (Variable) o;
			return this.GetSemanticType().Equals(that.GetSemanticType())
				&& this.id == that.GetID();
		}
		return false;
	}

	public override LogicalForm Bind(int id, LogicalForm l) {
		if (this.id == id && l.GetSemanticType().Equals(this.GetSemanticType())) return l;
		else return this;
	}
}
