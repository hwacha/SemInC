using System.Collections;
using System.Collections.Generic;

public class ToDesirability : LogicalForm {
    private LogicalForm sentence;
    
    public ToDesirability(LogicalForm sentence) : base(new W()) {
        if (sentence.GetType() != typeof(TWG)) {
            // error!
        }
        this.sentence = sentence;
        this.isFormula = true;
        this.freeVariables = sentence.GetFreeVariables();
    }

    public override ISemanticValue Denotation(Model m) {
        return ((TruthDesirabilityGoalValue) sentence.Denotation(m)).GetDesirability();
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new ToDesirability(sentence.Bind(id, l));
    }
}
