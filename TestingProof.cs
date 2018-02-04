using System;

public class Testing {

    static void TestLogicalForms() {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing LogicalForms:");
        Constant c = new Constant(new E(), 4);
        System.Console.WriteLine(c);
        System.Console.WriteLine("=====================");
    }

    static void TestFType() {
        System.Console.WriteLine("Testing FType:");
        Variable v = new Variable(new T(), 1);
        System.Console.WriteLine(new FType(new T(), v));
    }

    static void Main() {
        System.Console.WriteLine("TESTING PROOF");
        TestLogicalForms();
        TestFType();
    }
}
