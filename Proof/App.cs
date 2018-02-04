using System;
using System.Collections;
using System.Collections.Generic;

public class App : LogicalForm {
    private LogicalForm f;
    private LogicalForm x;

    public App(LogicalForm f, LogicalForm x) : base(null) {
        ISemanticType fType = f.GetSemanticType();
        ISemanticType xType = x.GetSemanticType();
        if (fType.GetType() != typeof(Arrow)) {
            System.Console.WriteLine(
                "App failed: function argument not function type");
            return;
        }
        Arrow aType = (Arrow) fType;

        if (!aType.GetInputType().Equals(xType)) {
            // error
            System.Console.WriteLine("App failed: type mismatch");
            // throw new InvalidTypeException();
            return;
        }

        this.type = aType.GetOutputType();

        this.isFormula = this.type.GetType() == typeof(T);

        this.f = f;
        this.x = x;
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new App(f.Bind(id, l), x.Bind(id, l));
    }
}
