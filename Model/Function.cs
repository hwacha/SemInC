using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using proof.Verum;
//import syntax.Expression;
//import syntax.S;
//package model;

public class Function : ISemanticValue {

    private int id;
    private Dictionary<ISemanticValue, ISemanticValue> map = new Dictionary<ISemanticValue, ISemanticValue>();

    public Function(int id)
    {
        this.id = id;
    }

    public Function()
    {
        this.id = -1;
    }

    public void Set(ISemanticValue input, ISemanticValue output)
    {
        map[input] = output;
    }

    public ISemanticValue Apply(ISemanticValue input)
    {
        return map[input];
    }

    public int GetID()
    {
        return id;
    }

    public Dictionary<ISemanticValue, ISemanticValue>.KeyCollection Domain()
    {
        return map.Keys;
    }

    public Dictionary<ISemanticValue, ISemanticValue>.ValueCollection Codomain()
    {
        return map.Values;
    }

 //   private string ToString(int depth)
 //   {
 //       string tab = mult("  ", depth + 1);
 //       StringBuilder s = new StringBuilder();
 //       //s.append("{\n");
 //       s.Append("\n");
 //       for (SemanticValue input : map.keySet())
 //       {
 //           s.Append(tab);
 //           if (input instanceof Function) {
 //           s.append(((Function)input).toString(depth + 1));
 //       } else 
 //           s.append(input);
 //       }
 //       s.append(" -> ");
 //       SemanticValue output = map.get(input);
 //       if (output instanceof Function) {
 //           s.append(((Function)output).toString(depth + 2));
 //       } else {
 //           s.append(output);
 //       }
 //       s.append("\n");
 //   }
	////s.append(tab + "}");
	//	return s.ToString();
	//}

private string Mult(string str, int n)
{
    StringBuilder s = new StringBuilder();
    for (int i = 0; i < n; i++)
    {
        s.Append(str);
    }
    return s.ToString();
}

//public string ToString()
//{
//    return ToString(0);
//}

    public bool Update(ISemanticValue that)
{
    if (!(that.GetType() == typeof(Function))) {
        return false;
    }
    Function other = (Function)that;

    bool hasUpdated = false;

    foreach (ISemanticValue k in other.map.Keys)
    {
        if (this.map.ContainsKey(k))
        {
            hasUpdated = this.map[k].Update(map[k]) || hasUpdated;
        }
        else
        {
            this.map[k] = map[k];
            hasUpdated = true;
        }
    }

    return hasUpdated;
}

    public ISemanticValue SClone()
{
    Function f = new Function(id);
    foreach (ISemanticValue k in map.Keys)
    {
        f.Set(k.SClone(), map[k].SClone());
    }
    return f;
}
}
