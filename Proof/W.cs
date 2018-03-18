using System;
using System.Collections;
using System.Collections.Generic;

// set of desirability values
public class W : ISemanticType {
    public override bool Equals(Object o) {
        return o.GetType() == typeof(W);
    }
}
