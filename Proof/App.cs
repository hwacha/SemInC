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

        this.freeVariables = f.MergeVariables(x);

        this.f = f;
        this.x = x;
    }

    public LogicalForm Reduce() {
        if (f.GetType() == typeof(Lambda)) {
            Lambda l = (Lambda) f;
            System.Console.WriteLine("the type IS a lambda");
            return l.Apply(x);
        }
        System.Console.WriteLine("the type is NOT a lambda");
        return this;
    }

    public override LogicalForm Bind(int id, LogicalForm l) {
        return new App(f.Bind(id, l), x.Bind(id, l));
    }

    public override ISemanticValue Denotation(Model m) {
        if (!IsClosed()) {
            return null;
        }
        
        if (f.GetType() == typeof(Lambda)) {
            return this.Reduce().Denotation(m);
        }
        
        ISemanticValue d = f.Denotation(m);

        if (d.GetType() == typeof(Function)) {
            return ((Function) d).Apply(x.Denotation(m));
        }

        return null;
    }

    public override string ToString() {
        return "(" + f.ToString() + " " + x.ToString() + ")";
    }
}
