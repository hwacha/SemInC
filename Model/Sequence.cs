using System;
using System.Collections;
using System.Collections.Generic;

public class Sequence : Policy {
    private List<Policy> ps;
    
    public Sequence(List<Policy> ps) {
        this.ps = ps;
    }

    public void Queue(Policy p) {
        ps.Add(p);
    }
    
    public List<Policy> GetPs() {
        return ps;
    }

    public override Policy GetDual() {
        List<Policy> newPs = new List<Policy>();
        
        foreach (Policy p in this.ps) {
            newPs.Add(p.GetDual());
        }

        return new Sequence(newPs);
    }

    public override Policy Concat(Policy pol) {
        List<Policy> newPs = new List<Policy>();

        foreach (Policy p in this.ps) {
            newPs.Add(p);
        }

        if (pol.GetType() == typeof(Sequence)) {
            foreach (Policy x in ((Sequence) pol).GetPs()) {
                newPs.Add(x);
            }    
        } else {
            newPs.Add(pol);
        }
        
        return new Sequence(newPs);
    }
}
