/// <summary>
/// Author:      Hannah Larsen
/// Partner:     None
/// Date:
/// Course:      CS3500, University of Utah, School of Computing
/// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
/// 
/// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
/// All references used in the completion of the assignment are cited in my README file.
/// 
/// File Contents:
/// This file contains a console application that tests the funtionality of the Evaluate function in the FormulaEvaluator
/// project. It uses print statements to verify that a test either passed or failed.
/// 
/// </summary>


void testPlus()
{
    if (FormulaEvaluator.Evaluator.Evaluate("3+5", null) == 8) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3+5+2", null) == 10) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3 + 5", null) == 8) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate(" 3  + 5 ", null) == 8) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testMinus()
{
    if (FormulaEvaluator.Evaluator.Evaluate("5-3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5-3-2", null) == 0) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5 - 3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5  -  3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testTimes()
{
    if (FormulaEvaluator.Evaluator.Evaluate("3*5", null) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3*5*2", null) == 30) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3 * 5", null) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testDivide()
{
    if (FormulaEvaluator.Evaluator.Evaluate("10/5", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("30/5/2", null) == 3) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("10/ 5", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testParenthesis()
{
    if (FormulaEvaluator.Evaluator.Evaluate("(3+5)*2", null) == 16) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(3 + 5) * 2", null) == 16) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("2  +(7- 1) / 2", null) == 5) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testMixOfOperations()
{
    if (FormulaEvaluator.Evaluator.Evaluate("5+10/2-1", null) == 9) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5+10/2-1*3", null) == 7) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(7)", null) == 7) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(2+5+(2*2)*(3*3))", null) == 43) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(2+ 5) /  2* (4-2)", null) == 6) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testVariables()
{
    if (FormulaEvaluator.Evaluator.Evaluate("3 * t2", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3 * (Pgn2+2)", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 21) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(  nK265+ 5)/5", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


void testIllegalExpressions()
{
    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3++5", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3++5");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("-3+5", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression -3+5");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3+(-5)", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3+(-5)");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * ((7+2)", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3 * ((7+2)");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * (7+2))", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3 * (7+2))");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * ()", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3 * ()");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * (5", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3 * (5");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * 5)", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3 * 5)");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("5(3+5)", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 5(3+5)");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("5+3$", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 5+3$");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("5+3/0", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 5+3/0");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression ''");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate(null, null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid null expression");
    }
}




// invalid variable testing:

void testInvalidVariables()
{
    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3ab", new FormulaEvaluator.Evaluator.Lookup(s => 5));
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3ab");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("ab4c", new FormulaEvaluator.Evaluator.Lookup(s => 5));
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression ab4c");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * t2", new FormulaEvaluator.Evaluator.Lookup(s => throw new ArgumentException()));
    }
    catch (ArgumentException)
    {
        Console.WriteLine("the delegate threw");
    }

}

testPlus();

testMinus();

testTimes();

testDivide();

testParenthesis();

testMixOfOperations();

testVariables();

testIllegalExpressions();

testInvalidVariables();

Console.Read();


