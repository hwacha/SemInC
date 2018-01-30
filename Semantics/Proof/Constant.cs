using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : LogicalForm {
	private ISemanticType type;
	private int id;

	public Constant(ISemanticType type, int id) {
		this.type = type;
		this.id = id;
	}

	public ISemanticType getType() {
		return type;
	}

	public bool isClosed() {
		return true;
	}

	public ISemanticValue denotation(Model m) {
		return m.get(id);
	}
}
