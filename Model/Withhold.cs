using System;
using System.Collections;
using System.Collections.Generic;

public class Withhold : Policy {
    private LogicalForm l;
    
    public Withhold(LogicalForm l) {
        this.l = l;
    }
    
     public override Policy GetDual() {
        return new Add(l);
    }
}
