using System;
using System.Collections;
using System.Collections.Generic;

public class Wrapper : ISemanticValue {
    private LogicalForm l;

    public Wrapper(LogicalForm l) {
        this.l = l;
    }

    public int GetID() {
        return -1;
    }

    public LogicalForm Get() {
        return l;
    }

    // public bool Update(ISemanticValue that) {
    //     return false;
    // }

    // public ISemanticValue SClone() {
    //     return new Wrapper(l);
    // }

    public override bool Equals(Object o) {
        if (o.GetType() == typeof(Wrapper)) {
            return l.Equals(((Wrapper) o).Get());
        }
        return false;
    }
}
