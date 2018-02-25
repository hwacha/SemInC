using System;
using System.Collections;
using System.Collections.Generic;

//set of truth values
public class T : ISemanticType {
    public override bool Equals(Object o) {
        return o.GetType() == typeof(T);
    }
}
