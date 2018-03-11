using System;

public class Testing {

    // check denotations for applications
    // when you add to model, make sure that shit stays there
    // make this sentence true/false
    // Rule.GetInference gets correct inference
    // denotation --> make sure logical form refers to correct thing in model
    // logical forms that are sentences ---> model.Satisfies returns right truth value, check for wrapper logical forms
    // super model calls satisfies
    // model inheritence
    // Make, Add & Denotation only affect lowest, Satisfies & Get Domain should affect everything up the chain

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
        //Or orLogForm = new Or(c1, c2); //an Or logical form
        //Wrapper wrappedOr = new Wrapper(orLogForm);
        //m.Add(new T(), 777, wrappedOr);
        //System.Console.WriteLine(wrappedOr.Equals(orLogForm.Denotation(m)));
        System.Console.WriteLine("====================");
    }

    // Heidi will come back to this
    static void TestAddRule()
    {
        //System.Console.WriteLine("=====================");
        //System.Console.WriteLine("Testing Add Rule:");
        //Model m = new Model();
        //// reflexivity

        //LogicalForm reflexivityFormula = new App(new App(containsRef, iv), iv);
        //reflexive.addBottom(reflexivityFormula);
        //Rule reflexive = new Rule();

        //// anti-symmetry
        //Rule antiSymmetric = new Rule();
        //LogicalForm antiSymmetricAnte = new Application(new Application(containsRef, iv), tv);
        //LogicalForm identity = new Equals(iv, tv);
        //LogicalForm antiSymmetricCons = new Application(new Application(containsRef, tv), iv);
        //antiSymmetric.AddTop(antiSymmetricAnte);
        //antiSymmetric.AddTop(antiSymmetricCons);
        //antiSymmetric.AddBottom(identity);

        //// transitivity
        //Variable v3 = new Variable(new E(), 2);
        //Rule transitive = new Rule();
        //LogicalForm t1 = new Application(new Application(containsRef, iv), tv);
        //LogicalForm t2 = new Application(new Application(containsRef, tv), v3);
        //LogicalForm t3 = new Application(new Application(containsRef, iv), v3);
        //transitive.AddTop(t1);
        //transitive.AddTop(t2);
        //transitive.AddBottom(t3);

        //// exclusion
        //Rule exclusive = new Rule();
        //LogicalForm e1 = new Application(new Application(containsRef, iv), tv);
        //LogicalForm e2 = new Application(new Application(containsRef, v3), tv);
        //LogicalForm e3 = new Application(new Application(containsRef, iv), v3);
        //LogicalForm e4 = new Application(new Application(containsRef, v3), iv);
        //exclusive.AddTop(e1);
        //exclusive.AddTop(e2);
        //exclusive.AddBottom(e3);
        //exclusive.AddBottom(e4);

        //// positive born-in rule
        //Rule bornEntail = new Rule();
        //LogicalForm b1 = new Application(new Application(bornRef, iv), tv);
        //LogicalForm b2 = e2;
        //LogicalForm b3 = new Application(new Application(bornRef, iv), v3);
        //bornEntail.AddTop(b1);
        //bornEntail.AddTop(b2);
        //bornEntail.AddBottom(b3);

        //// negative born-in rule
        //Rule bornReject = new Rule();
        //LogicalForm r1 = b1;
        //LogicalForm r2 = new Application(new Application(bornRef, iv), v3);
        //LogicalForm r3 = e2;
        //LogicalForm r4 = t2;
        //bornReject.AddTop(r1);
        //bornReject.AddTop(r2);
        //bornReject.AddBottom(r3);
        //bornReject.AddBottom(r4);

        //m.Update(bornEntail);
        //m.Update(bornReject);

        //m.Add(reflexive, antiSymmetric, transitive, exclusive, bornEntail, bornReject);
        //m.Update();

        //// Variable v = new Variable(new T(), 1);
        //// FType form = new FType(new T(), v);
        //// System.Console.WriteLine(form);
        //// System.Console.WriteLine("The formula is a formula?: " + form.GetFormula().IsFormula());
        //// rule.AddTop(form.GetFormula()); //adding a formula to the top of a rule
        //// rule.AddBottom(form.GetFormula()); //adding that same formula to the bottom of a rule
        //// m.Add(rule);
        //// System.Console.WriteLine(m);
        //System.Console.WriteLine("=====================");
    }
    
    //come back to this 
    static void TestMake()
    {
        System.Console.WriteLine("=====================");
        System.Console.WriteLine("Testing Make:");
        Model superModel = new Model();
        Model subModel = new Model(superModel);

        //make a new Rule with 2 things on top, 1 thing on bottom
        HashSet<LogicalForm> top = new HashSet<LogicalForm>();
        Variable varTop1 = new Variable(new E(), 200);
        Variable varTop2 = new Variable(new E(), 400);
        top.Add(varTop1);
        top.Add(varTop2);
        HashSet<LogicalForm>[] bot = new HashSet<LogicalForm>[1];
        Variable varBot1 = new Variable(new E(), 100);
        HashSet<LogicalForm> bot1 = new HashSet<LogicalForm>();
        bot1.Add(varBot1);
        bot[0] = bot1;
        Rule rule1 = new Rule(top, bot);

        Constant c1 = new Constant(new T(), 40);
        c1.Make(m, TruthValue.T.False); 

        //    Variable v = new Variable(new T(), 1);
        //    FType form = new FType(new T(), v);

        //    Constant c2 = new Constant(new T(), 20); //bottom of some bs rule
        //    rule.AddTop(c1);
        //    rule.AddBottom(c2);
        //    System.Console.WriteLine(form);
        //    System.Console.WriteLine("The formula is a formula?: " + form.GetFormula().IsFormula());
        //    rule.AddTop(form.GetFormula()); //adding a formula to the top of a rule
        //    rule.AddBottom(form.GetFormula()); //adding that same formula to the bottom of a rule
        //    m.Add(rule);
        //    System.Console.WriteLine(m);
        System.Console.WriteLine("=====================");
    }

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
      //  TestDenotation();
    }
}
