using System;
using System.Collections;
using System.Collections.Generic;

public class ModelBind : LogicalForm {
    private LogicalForm sentence;
    private LogicalForm model;

    public ModelBind(LogicalForm sentence, LogicalForm model)
    : base(new TWG()) {
        if (!sentence.IsFormula()) {
            // error!
        }
        if (model.GetType() != typeof(Model)) {
            // error!
        }
        this.sentence = sentence;

        this.model = model;

        this.isFormula = false;
        this.freeVariables = sentence.MergeVariables(model);

    }

    public override ISemanticValue Denotation(Model m) {
        return sentence.Denotation((Model) model.Denotation(m));
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new ModelBind(sentence.Bind(id, l), model.Bind(id, l));
    }
}
