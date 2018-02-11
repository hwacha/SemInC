using System;
using System.Collections;
using System.Collections.Generic;

public class Individual : ISemanticValue{

    private int id;
    //private Expression name;

    public Individual(int id)
    {
        this.id = id;
    }

    public int GetID()
    {
        return id;
    }

    public int HashCode()
    {
        return id;
    }

    //public void SetName(Expression e)
    //{
      //  this.name = e;
    //}

    public override bool Equals(Object o) {
        return (o.GetType() == typeof(Individual)) && (((Individual)o).id == this.id);
    }

    public bool Update(ISemanticValue that)
    {
        return false;
    }

    public ISemanticValue SClone()
    {
        return new Individual(id);
    }
}
//import syntax.Expression;

