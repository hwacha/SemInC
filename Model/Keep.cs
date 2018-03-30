using System;
using System.Collections;
using System.Collections.Generic;

public class Keep : Policy {
    private LogicalForm l;
    
    public Keep(LogicalForm l) {
        this.l = l;
    }
    
     public override Policy GetDual() {
        return new Keep(l);
    }
}
