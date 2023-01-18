// + operator testing (no spaces):

if (FormulaEvaluator.Evaluator.Evaluate("3+5", null) == 8) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("3+5+2", null) == 10) Console.WriteLine("passed");
else Console.WriteLine("failed");




// + operator testing (with spaces):

if (FormulaEvaluator.Evaluator.Evaluate("3 + 5", null) == 8) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate(" 3  + 5 ", null) == 8) Console.WriteLine("passed");
else Console.WriteLine("failed");




// - operator testing (no spaces):

if (FormulaEvaluator.Evaluator.Evaluate("5-3", null) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("5-3-2", null) == 0) Console.WriteLine("passed");
else Console.WriteLine("failed");




// - operator testing (with spaces):

if (FormulaEvaluator.Evaluator.Evaluate("5 - 3", null) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("5  -  3", null) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");




// * operator testing (no spaces):

if (FormulaEvaluator.Evaluator.Evaluate("3*5", null) == 15) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("3*5*2", null) == 30) Console.WriteLine("passed");
else Console.WriteLine("failed");




// * operator testing (with spaces):

if (FormulaEvaluator.Evaluator.Evaluate("3 * 5", null) == 15) Console.WriteLine("passed");
else Console.WriteLine("failed");




// / operator testing (no spaces):
if (FormulaEvaluator.Evaluator.Evaluate("10/5", null) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("30/5/2", null) == 3) Console.WriteLine("passed");
else Console.WriteLine("failed");




// / operator testing (with spaces):

if (FormulaEvaluator.Evaluator.Evaluate("10/ 5", null) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");




// parenthesis testing (no spaces):

if (FormulaEvaluator.Evaluator.Evaluate("(3+5)*2", null) == 16) Console.WriteLine("passed");
else Console.WriteLine("failed");




// parenthesis testing (with spaces):

if (FormulaEvaluator.Evaluator.Evaluate("(3 + 5) * 2", null) == 16) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("2  +(7- 1) / 2", null) == 5) Console.WriteLine("passed");
else Console.WriteLine("failed");




// mixture of operations testing:

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




// variable testing:

if (FormulaEvaluator.Evaluator.Evaluate("3 * t2", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 15) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("3 * (Pgn2+2)", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 21) Console.WriteLine("passed");
else Console.WriteLine("failed");
if (FormulaEvaluator.Evaluator.Evaluate("(  nK265+ 5)/5", new FormulaEvaluator.Evaluator.Lookup(s => 5)) == 2) Console.WriteLine("passed");
else Console.WriteLine("failed");




// illegal expression testing below:

try
{
    FormulaEvaluator.Evaluator.Evaluate("3++5", null);
} catch (ArgumentException)
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




// invalid variable testing:

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

Console.Read();


