using System;
using System.Collections;
using System.Collections.Generic;

public class FType : ISemanticType {
    private ISemanticType basetype;
    private LogicalForm formula;

    public FType(ISemanticType basetype, LogicalForm formula) {
        if (!formula.IsFormula() || formula.IsClosed()) {
            System.Console.WriteLine("not a formula, or not free");
            return;
            // error
        }
        HashSet<Variable>.Enumerator vars = formula.GetFreeVariables().GetEnumerator();
        vars.MoveNext();
        Variable first = vars.Current;
        if (vars.MoveNext()) {
            System.Console.WriteLine("more than one free variable");
            return;
            // error (more than one free variable)
        }

        if (!first.GetSemanticType().Equals(basetype)) {
            System.Console.WriteLine(first.GetSemanticType());
            System.Console.WriteLine(basetype);

            System.Console.WriteLine("types aren't equal");
            return;
            // error
        }
        System.Console.WriteLine("made properly");
        this.basetype = basetype;
        this.formula = formula;
    }

    public ISemanticType GetBaseType() {
        return basetype;
    }

    public LogicalForm GetFormula() {
        return formula;
    }
}
