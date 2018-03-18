using System;
using System.Collections;
using System.Collections.Generic;

// set of goal values
public class G : ISemanticType {
    public override bool Equals(Object o) {
        return o.GetType() == typeof(G);
    }
}
