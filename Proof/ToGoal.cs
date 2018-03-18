using System.Collections;
using System.Collections.Generic;

public class ToGoal : LogicalForm {
    private LogicalForm sentence;
    
    public ToGoal(LogicalForm sentence) : base(new G()) {
        this.sentence = sentence;
        this.isFormula = true;
        this.freeVariables = sentence.GetFreeVariables();
    }

    public override ISemanticValue Denotation(Model m) {
        return ((TruthDesirabilityGoalValue) sentence.Denotation(m)).GetGoal();
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new ToGoal(sentence.Bind(id, l));
    }
}
