using System;

public class Testing {
    static void TestTruthValues() {
        System.Console.WriteLine("====================");
        System.Console.WriteLine("Testing TruthValues:");
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
        System.Console.WriteLine("====================");
    }

    static void Main() {
        System.Console.WriteLine("TESTING MODEL");
        TestTruthValues();
    }
}
