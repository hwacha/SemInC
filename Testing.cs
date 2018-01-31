using System;

public class Testing {
    static void Main() {
        System.Console.WriteLine("this is testing, not constant");
        TruthValue v = new TruthValue();
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(true));
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(true));
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(false));
        System.Console.WriteLine(v.toString());
        v.Clear();
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(false));
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(false));
        System.Console.WriteLine(v.toString());
        System.Console.WriteLine(v.Add(true));
        System.Console.WriteLine(v.toString());
    }
}
