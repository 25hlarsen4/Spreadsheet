/// <summary>
/// Author:      Hannah Larsen
/// Partner:     None
/// Date:        13-Jan-2023
/// Course:      CS3500, University of Utah, School of Computing
/// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
/// 
/// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
/// All references used in the completion of the assignment are cited in my README file.
/// 
/// File Contents:
/// This file contains a console application that tests the functionality of the Evaluate function in the FormulaEvaluator
/// project. It uses print statements to verify that a test either passed or failed.
/// 
/// </summary>


/// <summary>
/// This method tests the Evaluate method on mathematical expressions only involving the + operator, both with and without 
/// whitespaces in the expressions.
/// </summary>
void TestPlus()
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


/// <summary>
/// This method tests the Evaluate method on mathematical expressions only involving the - operator, both with and without 
/// whitespaces in the expressions.
/// </summary>
void TestMinus()
{
    if (FormulaEvaluator.Evaluator.Evaluate("5-3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5-3-2", null) == 0) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5 - 3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5  -  3", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5 - 3 - 2", null) == 0) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5 - (8 - 2)", null) == -1) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests the Evaluate method on mathematical expressions only involving the * operator, both with and without 
/// whitespaces in the expressions.
/// </summary>
void TestTimes()
{
    if (FormulaEvaluator.Evaluator.Evaluate("3*5", null) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3*5*2", null) == 30) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3 * 5", null) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests the Evaluate method on mathematical expressions only involving the / operator, both with and without 
/// whitespaces in the expressions.
/// </summary>
void TestDivide()
{
    if (FormulaEvaluator.Evaluator.Evaluate("10/5", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("30/5/2", null) == 3) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("10/   5", null) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests the Evaluate method on mathematical expressions only involving the parenthesis operators, both with 
/// and without whitespaces in the expressions.
/// </summary>
void TestParenthesis()
{
    if (FormulaEvaluator.Evaluator.Evaluate("(3+5)*2", null) == 16) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(3 + 5    ) * 2", null) == 16) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("2   +(7- 1) / 2", null) == 5) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests the Evaluate method on mathematical expressions involving a mixture of all operators, meaning +, -, *, /, 
/// (, and ), both with and without whitespaces in the expressions.
/// </summary>
void TestMixOfOperations()
{
    if (FormulaEvaluator.Evaluator.Evaluate("5+10/2-1", null) == 9) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("5+10/2-1*3", null) == 7) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(7)", null) == 7) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(3+5+(2*2)*(3*3))", null) == 44) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(2+ 5) /  2* (4-2)", null) == 6) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("((3+5+(2*2)*(3*3)) /2) *3", null) == 66) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(((3 - 4) - 2) * 3) / 2", null) == -4) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests the Evaluate method on mathematical expressions involving variables, both with and without 
/// whitespaces in the expressions.
/// </summary>
void TestVariables()
{
    //if (FormulaEvaluator.Evaluator.Evaluate("3 * t2", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 15) Console.WriteLine("passed");
    if (FormulaEvaluator.Evaluator.Evaluate("3 * t2", (s) => 5) == 15) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("3 * (Pgn2+2)", (s) => 5) == 21) Console.WriteLine("passed");
    else Console.WriteLine("failed");

    if (FormulaEvaluator.Evaluator.Evaluate("(  nK265+ 5)/5", (s) => 5) == 2) Console.WriteLine("passed");
    else Console.WriteLine("failed");
}


/// <summary>
/// This method tests that the Evaluate method throws ArugmentExceptions on illegal mathematical expressions.
/// </summary>
void TestIllegalExpressions()
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
        FormulaEvaluator.Evaluator.Evaluate("(3+5", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression (3+5");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3+)(5)", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression 3+)(5)");
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
        FormulaEvaluator.Evaluator.Evaluate("((3+5+(2*2)*(3*3)) / (2-2)) *3", null);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid expression ((3+5+(2*2)*(3*3)) / (2-2) *3");
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


/// <summary>
/// This method tests that the Evaluate method throws ArugmentExceptions on mathematical expressions
/// that include invalid variables.
/// </summary>
void TestInvalidVariables()
{
    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3ab", (s) => 5);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid variable 3ab");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("ab4c", (s) => 5);
    }
    catch (ArgumentException)
    {
        Console.WriteLine("invalid variable ab4c");
    }

    try
    {
        FormulaEvaluator.Evaluator.Evaluate("3 * t2", (s) => throw new ArgumentException());
    }
    catch (ArgumentException)
    {
        Console.WriteLine("the delegate threw");
    }

}

TestPlus();

TestMinus();

TestTimes();

TestDivide();

TestParenthesis();

TestMixOfOperations();

TestVariables();

TestIllegalExpressions();

TestInvalidVariables();

Console.Read();


