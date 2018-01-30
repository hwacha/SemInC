using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LogicalForm {
	ISemanticType GetSemanticType();
	ISemanticValue Denotation(Model m);

	bool IsFormula();
	bool IsClosed();

	ILogicalForm Bind(int id, ILogicalForm l);
	HashSet<Variable> GetFreeVariables(HashSet<Variable> bound);
}
