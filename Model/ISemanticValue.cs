using System.Collections;
using System.Collections.Generic;

public interface ISemanticValue {
    int GetID();
    bool Update(ISemanticValue that);
    ISemanticValue SClone();
}
