using System.Collections;
using System.Collections.Generic;

public abstract class Policy {
    // int CompareLikelihood(Policy p, Model m);
    public abstract Policy GetDual();

    public virtual Policy Concat(Policy p) {
        List<Policy> newPs = new List<Policy>();

        newPs.Add(this);

        if (p.GetType() == typeof(Sequence)) {
            foreach (Policy x in ((Sequence) p).GetPs()) {
                newPs.Add(x);
            }
        } else {
            newPs.Add(p);  
        }
        
        return new Sequence(newPs);
    }
}
