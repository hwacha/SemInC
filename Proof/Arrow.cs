using System;
using System.Collections;
using System.Collections.Generic;

public class Arrow : ISemanticType {
    private ISemanticType input;
    private ISemanticType output;

    public Arrow(ISemanticType input, ISemanticType output) {
        this.input = input;
        this.output = output;
    }

    public ISemanticType GetInputType() {
        return input;
    }

    public ISemanticType GetOutputType() {
        return output;
    }

    public override bool Equals(Object o) {
        if (o.GetType() != typeof(Arrow)) {
            return false;
        }

        Arrow that = (Arrow) o;

        return this.input.Equals(that.input)
            && this.output.Equals(that.output);
    }

    public override string ToString() {
        return "(" + input.ToString()
            + "->" + output.ToString() + ")";
    }
}
