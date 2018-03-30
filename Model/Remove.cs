using System;
using System.Collections;
using System.Collections.Generic;

public class Remove : Policy {
    private LogicalForm l;
    
    public Remove(LogicalForm l) {
        this.l = l;
    }

    public override Policy GetDual() {
        return new Keep(l);
    }
}