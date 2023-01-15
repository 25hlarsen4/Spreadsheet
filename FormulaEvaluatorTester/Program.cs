// + operator testing (no spaces)
Console.WriteLine("3 + 5 = " + FormulaEvaluator.Evaluator.Evaluate("3+5", null));

// + operator testing (with spaces)
Console.WriteLine("3 + 5 = " + FormulaEvaluator.Evaluator.Evaluate("3 + 5", null));


// - operator testing (no spaces)
Console.WriteLine("5 - 3 = " + FormulaEvaluator.Evaluator.Evaluate("5-3", null));

// - operator testing (with spaces)
Console.WriteLine("5 - 3 = " + FormulaEvaluator.Evaluator.Evaluate("5 - 3", null));


// * operator testing (no spaces)
Console.WriteLine("3 * 5 = " + FormulaEvaluator.Evaluator.Evaluate("3*5", null));

// * operator testing (with spaces)
Console.WriteLine("3 * 5 = " + FormulaEvaluator.Evaluator.Evaluate("3 * 5", null));


// / operator testing (no spaces)
Console.WriteLine("10 / 5 = " + FormulaEvaluator.Evaluator.Evaluate("10/5", null));

// / operator testing (with spaces)
Console.WriteLine("10 / 5 = " + FormulaEvaluator.Evaluator.Evaluate("10 / 5", null));


// parenthesis testing (no spaces)
Console.WriteLine("(3 + 5) * 2 = " + FormulaEvaluator.Evaluator.Evaluate("(3+5)*2", null));

// parenthesis testing (with spaces)
Console.WriteLine("(3 + 5) * 2 = " + FormulaEvaluator.Evaluator.Evaluate("(3 + 5) * 2", null));


// illegal expression testing below
try
{
    FormulaEvaluator.Evaluator.Evaluate("3++5", null);
} catch (ArgumentException)
{
    Console.WriteLine("invalid expression 3++5");
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


// 5++3     5(3+5)     5+3$



Console.Read();


