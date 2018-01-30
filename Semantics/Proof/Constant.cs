using System.Collections;
using System.Collections.Generic;

public class Constant : LogicalForm {
	private ISemanticType type;
    private int id;

	public Constant(ISemanticType type, int id) {
		this.type = type;
		this.id = id;
        this.isFormula = type.GetType() == typeof(T);
	}

    public override LogicalForm Bind(int id, LogicalForm l) {
        return this;
    }

	//public ISemanticValue Denotation(Model m) {
	//	return m.get(id);
	//}

    public static void Main() {
        System.Console.WriteLine("Heyo");
    }
}
