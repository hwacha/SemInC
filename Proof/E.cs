﻿using System;
using System.Collections;
using System.Collections.Generic;

//set of individuals
public class E : ISemanticType {
    public override bool Equals(Object o) {
        return o.GetType() == typeof(E);
    }
}
