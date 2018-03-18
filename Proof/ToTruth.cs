using System.Collections;
using System.Collections.Generic;

public class ToTruth : LogicalForm {
    private LogicalForm sentence;
    
    public ToTruth(LogicalForm sentence) : base(new T()) {
        this.sentence = sentence;
        this.isFormula = true;
        this.freeVariables = sentence.GetFreeVariables();
    }

    public override ISemanticValue Denotation(Model m) {
        return ((TruthDesirabilityGoalValue) sentence.Denotation(m)).GetTruth();
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new ToTruth(sentence.Bind(id, l));
    }
}
