using System;
using System.Collections;
using System.Collections.Generic;

public class Add : Policy {
    private LogicalForm l;
    
    public Add(LogicalForm l) {
        this.l = l;
    }

    public override Policy GetDual() {
        return new Remove(l);
    }
}
