using System;

public class Testing {


    //denotation --> make sure logical form refers to correct thing in model
    // logical forms taht are sentences ---> model.Satisfies returns right truth value, check for wrapper logical forms
    // super model calls satisfies (not going up chain)
    //get domain; want to get all the individuals up the chain

    static void TestTruthValues() {
        System.Console.WriteLine("====================");
        System.Console.WriteLine("Testing Model:");
        Model m = new Model();
        //System.Console.WriteLine(v.Add(true));
        //System.Console.WriteLine(v.toString());
        System.Console.WriteLine("====================");
    }

    static void TestDenotation() {
        System.Console.WriteLine("====================");
        System.Console.WriteLine("Testing Denotation:");
        Model m = new Model();
        Constant c1 = new Constant(new T(), 666);
        Constant c2 = new Constant(new T(), 999);
        Or orLogForm = new Or(c1, c2); //an Or logical form
        Wrapper wrappedOr = new Wrapper(orLogForm);
        m.Add(new T(), 777, wrappedOr);
        System.Console.WriteLine(wrappedOr.Equals(orLogForm.Denotation(m)));
        System.Console.WriteLine("====================");
    }

    static void TestAddRule()
    {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Add Rule:");
        Model m = new Model();
        Rule rule = new Rule();
        Constant c1 = new Constant(new T(), 40); //top of some bs rule
        Constant c2 = new Constant(new T(), 20); //bottom of some bs rule
        rule.AddTop(c1);
        rule.AddBottom(c2);
        Variable v = new Variable(new T(), 1);
        FType form = new FType(new T(), v);
        System.Console.WriteLine(form);
        System.Console.WriteLine("The formula is a formula?: " + form.GetFormula().IsFormula());
        rule.AddTop(form.GetFormula()); //adding a formula to the top of a rule
        rule.AddBottom(form.GetFormula()); //adding that same formula to the bottom of a rule
        m.Add(rule);
        System.Console.WriteLine(m);
        System.Console.WriteLine("=====================");
    }

    //come back to this 
    //static void TestMake()
    //{
    //    System.Console.WriteLine("=====================");
    //    System.Console.WriteLine("Testing Make:");
    //    Model m = new Model();
    //    Rule rule = new Rule();

    //    Variable v = new Variable(new T(), 1);
    //    FType form = new FType(new T(), v);

    //    Constant c1 = new Constant(new T(), 40); //top of some bs rule
    //    Constant c2 = new Constant(new T(), 20); //bottom of some bs rule
    //    rule.AddTop(c1);
    //    rule.AddBottom(c2);
    //    System.Console.WriteLine(form);
    //    System.Console.WriteLine("The formula is a formula?: " + form.GetFormula().IsFormula());
    //    rule.AddTop(form.GetFormula()); //adding a formula to the top of a rule
    //    rule.AddBottom(form.GetFormula()); //adding that same formula to the bottom of a rule
    //    m.Add(rule);
    //    System.Console.WriteLine(m);
    //    System.Console.WriteLine("=====================");
    //}

        //come back to this
    //static void TestUpdate()
    //{
    //    System.Console.WriteLine("=====================");
    //    System.Console.WriteLine("Testing Update:");
    //    Model m = new Model();
    //    Rule rule = new Rule();
    //    Constant c1 = new Constant(new T(), 40); //top of some bs rule
    //    Constant c2 = new Constant(new T(), 20); //bottom of some bs rule
    //    rule.AddTop(c1);
    //    rule.AddBottom(c2);
    //    Variable v = new Variable(new T(), 1);
    //    FType form = new FType(new T(), v);
    //    System.Console.WriteLine(form);
    //    System.Console.WriteLine("The formula is a formula?: " + form.GetFormula().IsFormula());
    //    rule.AddTop(form.GetFormula()); //adding a formula to the top of a rule
    //    rule.AddBottom(form.GetFormula()); //adding that same formula to the bottom of a rule
    //    m.Add(rule);
    //    System.Console.WriteLine(m);
    //    System.Console.WriteLine("=====================");
    //}

    static void Main() {
        System.Console.WriteLine("TESTING MODEL");
        TestTruthValues();
        TestAddRule();
        TestDenotation();
    }
}
