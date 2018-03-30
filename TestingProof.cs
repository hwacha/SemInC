using System;
using System.Collections.Generic;

public class Testing {

    // TODO:
    // variable binding [done]
    // merging free variables [done]
    // check App type correct [done]
    // test Rule

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
        App app = SetUpApp();
        System.Console.WriteLine(app);
        System.Console.WriteLine(app.GetSemanticType());

        System.Console.WriteLine("=====================");
    }
    
    // A method to test the Reduce() method of the App class
    static void TestReduceInApp()
    {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Reduce method in App:");
        App app = SetUpFxnApp();
        System.Console.WriteLine(app);
        System.Console.WriteLine(app.GetSemanticType());
        app.Reduce();
        System.Console.WriteLine("Reduced App is " + app.GetSemanticType());
        System.Console.WriteLine("=====================");
    }

    // A method to test the Rule class (not Model stuff)
    static void TestRule()
    {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Rule:");

        //make a new Rule with 3 things on top, 2 things on bottom
        HashSet<LogicalForm> top = new HashSet<LogicalForm>();
        Variable varTop1 = new Variable(new E(), 200);
        Variable varTop2 = new Variable(new E(), 400);
        Variable varTop3 = new Variable(new E(), 600);
        top.Add(varTop1);
        top.Add(varTop2);
        top.Add(varTop3);

        HashSet<LogicalForm>[] bot = new HashSet<LogicalForm>[2];
        Variable varBot1 = new Variable(new E(), 100);
        Variable varBot2 = new Variable(new E(), 300);
        HashSet<LogicalForm> bot1 = new HashSet<LogicalForm>();
        HashSet<LogicalForm> bot2 = new HashSet<LogicalForm>();
        bot1.Add(varBot1);
        bot2.Add(varBot2);
        bot[0] = bot1;
        bot[1] = bot2;

        Rule r1 = new Rule(top, bot);
        System.Console.WriteLine("The rule is: " + r1.ToString());

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

    static void TestMergingVariables() {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Merging Variables:");
        Variable var0 = new Variable(new E(), 0);
        Variable var1 = new Variable(new E(), 1);
        Variable var2 = new Variable(new E(), 2);
        Variable var3 = new Variable(new Arrow(new E(), new T()), 3);
        Variable var4 = new Variable(new Arrow(new E(), new T()), 4);

        // make 2 Lambdas with free variables & then check the size of their variables merged
        Lambda lambda1 = new Lambda(
            new Variable(new E(), 9),
            new App(var3, var0));
        System.Console.WriteLine("Lambda 1: " + lambda1);
        Lambda lambda2 = new Lambda(
            new Variable(new E(), 8),
            new App(var4, var1));
        System.Console.WriteLine("Lambda 2: " + lambda2);
        HashSet<Variable> varSet = lambda1.MergeVariables(lambda2);
        System.Console.WriteLine("The variable set contains 0? " + varSet.Contains(var0));
        System.Console.WriteLine("The variable set contains 1? " + varSet.Contains(var1));
        System.Console.WriteLine("The variable set contains 2? " + varSet.Contains(var2));
        System.Console.WriteLine("The variable set contains 3? " + varSet.Contains(var3));
        System.Console.WriteLine("The variable set contains 4? " + varSet.Contains(var4));
        System.Console.WriteLine("The variable count is: " + varSet.Count);
        System.Console.WriteLine("=====================");
    }

    // A method to test Variable binding
    static void TestVariableBinding()
    {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Variable Binding:");
        Variable var1 = new Variable(new E(), 01);
        Variable var2 = new Variable(new E(), 02);
        Variable var3 = new Variable(new T(), 03);

        if (var1.Equals(var1.Bind(123, var2)))
        {
            System.Console.WriteLine("Could not bind var1 to var2 because their IDs " +
                "were not equal; good! This should happen");
        }
        if (var1.Equals(var1.Bind(01, var3)))
        {
            System.Console.WriteLine("Could not bind var1 to var3 because their semantic types " +
                "were not equal; good! This should happen");
        }
        if (var2.Equals(var1.Bind(01, var2)))
        {
            System.Console.WriteLine("Successfully bound 2 variables with matching IDs and semantic types!");
        }
        System.Console.WriteLine("=====================");
    }

    // A method to set up an App for use by other testers
    static App SetUpApp()
    {
        return new App (new Constant(new Arrow(new E(), new T()), 6), new Constant(new E(), 7));
    }

    // A method to set up an App for use by other testers
    static App SetUpFxnApp()
    {
        Lambda l = new Lambda(
           new Variable(new E(), 0),
           new App(
               new Variable(new Arrow(new E(), new T()), 8),
               new Variable(new E(), 01)));
        return new App(l, new Variable(new E(), 909));
    }

    static void Main() {
        System.Console.WriteLine("TESTING PROOF");
        TestLogicalForms();
        TestFType();
        TestArrow();
        TestApp();
        TestLambda();
        TestReduceInApp();
        TestVariableBinding();
        TestMergingVariables();
        TestRule();
    }
}

