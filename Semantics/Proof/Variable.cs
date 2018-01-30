using System;

public class Variable {

	//private ISemanticType type;
	//private int id;

	//public Variable(ISemanticType type, int id) {
	//	this.type = type;
	//	this.id = id;
	//}

	//public ISemanticType GetSemanticType() {
	//	return type;
	//}

	//public override int GetID() {
	//	return id;
	//}

	//public override bool IsClosed() {
	//	return false;
	//}

	//public override ISemanticValue Denotation(Model m) {
	//	return null;
	//}

	//public override bool IsFormula() {
	//	return (type.GetType() == typeof(T));
	//}

	//public override String ToString() {
	//	return "{" + id + "}";
	//}

	//public override bool Equals(Object o) {
	//	if (o.GetType() == typeof(Variable)) {
	//		Variable that = (Variable) o;
	//		return this.GetSemanticType().Equals(that.GetSemanticType())
	//			&& this.id == that.GetID();
	//	}
	//	return false;
	//}

	//public override ILogicalForm bind(int id, ILogicalForm l) {
	//	if (this.id == id && l.GetSemanticType().Equals(this.GetSemanticType())) return l;
	//	else return this;
	//}
}
