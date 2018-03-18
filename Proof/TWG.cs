using System;
using System.Collections;
using System.Collections.Generic;

// set of (truth, desirability, goal) values
public class TWG : ISemanticType {
    public override bool Equals(Object o) {
        return o.GetType() == typeof(TWG);
    }
}
