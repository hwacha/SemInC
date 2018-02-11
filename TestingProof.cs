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
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing FType:");
        Variable v = new Variable(new T(), 1);
        System.Console.WriteLine(new FType(new T(), v));
        System.Console.WriteLine("=====================");
    }

    static void TestArrow() {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Arrow:");
        Arrow a = new Arrow(new E(), new T());
        System.Console.WriteLine(a);
        System.Console.WriteLine("=====================");
    }

    static void TestApp() {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing App:");
        App app =
        new App
        (new Constant(new Arrow(new E(), new T()), 6),
        new Constant(new E(), 7));

        System.Console.WriteLine(app);
        System.Console.WriteLine(app.GetSemanticType());

        System.Console.WriteLine("=====================");
    }

    static void TestLambda() {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Lambda:");
        Lambda l = new Lambda(
            new Variable(new E(), 0),
            new App(
                new Variable(new Arrow(new E(), new T()), 8),
                new Variable(new E(), 0)));

        System.Console.WriteLine(l);

        LogicalForm noChange = l.Bind(0, new Constant(new E(), 40));

        System.Console.WriteLine(noChange);

        LogicalForm variableReplace =
            l.Bind(8, new Constant(new Arrow(new E(), new T()), 60));

        System.Console.WriteLine(variableReplace);

        System.Console.WriteLine(l.Apply(new Constant(new E(), 40)));

        System.Console.WriteLine("=====================");
    }

    static void TestRule() {
        
    }

    static void Main() {
        System.Console.WriteLine("TESTING PROOF");
        TestLogicalForms();
        TestFType();
        TestArrow();
        TestApp();
        TestLambda();
    }
}
