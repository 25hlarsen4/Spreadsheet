// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens

using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Author:      Hannah Larsen
    /// Partner:     None
    /// Date:        27-Jan-2023
    /// Course:      CS3500, University of Utah, School of Computing
    /// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
    /// 
    /// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
    /// All references used in the completion of the assignment are cited in my README file.
    /// 
    /// File Contents:
    /// This file contains a class library that represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /. 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// This IEnumerable holds all tokens of a Formula.
        /// </summary>
        private IEnumerable<string> tokens;

        /// <summary>
        /// This func is a normalizer that is used to convert variables into a canonical form.
        /// </summary>
        Func<string, string> normalizer;

        /// <summary>
        /// This func is a validator that is used to add extra restrictions on the validity of a variable (beyond the standard requirement 
        /// that it consists of a letter or underscore followed by zero or more letters, underscores,
        /// or digits.)
        /// </summary>
        Func<string, bool> validator;


        /// <summary>
        /// This is a helper method that determines if the input string is a 
        /// variable by determining if it matches the variable regular expression.
        /// </summary>
        /// <param name="token"> The string to see if is a variable. </param>
        /// <returns> Returns true if the input string is a variable, and false
        /// otherwise. </returns>
        private static Boolean isVariable(String token)
        {
            if (Regex.IsMatch(token, @"^[a-zA-Z_][a-zA-Z\d_]*$"))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// This is a helper method meant to be called in the DetermineIfSyntacticallyIncorrect
        /// method below that throws an exception if a variable is illegal after being normalized or 
        /// invalid by the validator after being normalized.
        /// </summary>
        /// <param name="variable"> Represents the variable to determine the validity of. </param>
        /// <exception cref="FormulaFormatException"> Throws a FormulaFormatException if a 
        /// variable is found to be invalid. </exception>
        private void DetermineVariableValidity(string variable)
        {
            if (!isVariable(normalizer(variable)))
            {
                throw new FormulaFormatException("A normalized variable was found to be illegal. Make sure the normalized versions of your variables fits the pattern of a letter or underscore followed by zero or more letters, underscores, or digits.");
            }

            if (!validator(normalizer(variable)))
            {
                throw new FormulaFormatException("A normalized variable was found to be invalid. Make sure the normalized versions of your variables meet the conditions of your validator.");
            }
        }


        /// <summary>
        /// This is a helper method to be called in the Formula constructors below that will throw 
        /// a FormulaFormatException if this Formula is syntactically incorrect.
        /// </summary>
        /// <exception cref="FormulaFormatException"> Throws a FormulaFormatException if this 
        /// Formula is syntactically incorrect. </exception>
        private void DetermineIfSyntacticallyIncorrect()
        {
            IEnumerator<string> formTokens = tokens.GetEnumerator();
            int numOfLeftParenth = 0;
            int numOfRightParenth = 0;

            // there must be at least one token
            if (!formTokens.MoveNext())
            {
                throw new FormulaFormatException("The formula is empty. Make sure that the formula contains something to evaluate.");
            }

            string prevToken = formTokens.Current;

            // the first token must be a (, number, or variable
            if ((prevToken != "(") && !Double.TryParse(prevToken, out double result) && !isVariable(prevToken))
            {
                throw new FormulaFormatException("Formulas must start with either a number, a variable, or a (. Make sure that the formula starts with one of these things.");
            }

            // now check that if it's a variable, it is legal after being normalized and valid by the validator
            else if (isVariable(prevToken))
            {
                DetermineVariableValidity(prevToken);
            }

            if (prevToken == "(")
            {
                numOfLeftParenth += 1;
            }

            // now iterate through the rest of the tokens
            while (formTokens.MoveNext())
            {
                if (numOfRightParenth > numOfLeftParenth)
                {
                    throw new FormulaFormatException("The number of closing parenthesis was found to exceed the number of opening parenthesis at some point in the formula. Make sure that the number of closing parenthesis never exceeds the number of opening parenthesis.");
                }

                // if previous is an operator or (
                if ((prevToken == "(") || (prevToken == "+") || (prevToken == "-") || (prevToken == "*") || (prevToken == "/"))
                {
                    // current must be a number, variable, or (
                    if (!Double.TryParse(formTokens.Current, out double result2) && formTokens.Current != "(" && !isVariable(formTokens.Current))
                    {
                        throw new FormulaFormatException("Something other than a number, variable, or ( was found to be following an operator or (. Make sure that anything following an operator or ( is either a number, variable, or (.");
                    }

                    // if current is a variable, make sure it's valid
                    else if (isVariable(formTokens.Current))
                    {
                        DetermineVariableValidity(formTokens.Current);
                    }

                    else if (formTokens.Current == "(")
                    {
                        numOfLeftParenth += 1;
                    }
                }

                // if previous is number, variable, or )
                else if ((Double.TryParse(prevToken, out double result3)) || (prevToken == ")") || isVariable(prevToken))
                {
                    // current must be an operator or )
                    if ((formTokens.Current != "+") && (formTokens.Current != "-") && (formTokens.Current != "*") && (formTokens.Current != "/") && (formTokens.Current != ")"))
                    {
                        throw new FormulaFormatException("Something other than an operator or ) was found to be following a number, variable, or ). Make sure that anything following a number, variable, or ) is either an operator or ).");
                    }

                    else if (formTokens.Current == ")")
                    {
                        numOfRightParenth += 1;
                    }
                }

                prevToken = formTokens.Current;
            }

            // prevToken should now hold the last token, so check if it's valid
            if ((prevToken != ")") && !(Double.TryParse(prevToken, out double result4)) && !isVariable(prevToken))
            {
                throw new FormulaFormatException("Formulas must end with either a ), number, or variable. Make sure that the formula ends with one of these things.");
            }

            if (numOfLeftParenth != numOfRightParenth)
            {
                throw new FormulaFormatException("The number of opening and closing parenthesis was found to not be equal. Make sure that the number of opening parenthesis matches the number of closing parenthesis.");
            }
        }


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true. 
        /// </summary>
        /// <param name="formula"> Represents the expression. </param>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            tokens = Tokenize(formula);
            DetermineIfSyntacticallyIncorrect();
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// </summary>
        /// <param name="formula"> Represents the expression. </param>
        /// <param name="normalize"> Represents the variable normalizer. </param>
        /// <param name="isValid"> Represents the variable validator. </param>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            normalizer = normalize;
            validator = isValid;

            tokens = Tokenize(formula);
            DetermineIfSyntacticallyIncorrect();
        }


        /// <summary>
        /// This is a private helper method for the Evaluate algorithm that takes in two integer operands, a value stack, 
        /// and an operator stack, either multiplies or divides the two operands (depending on what is on top 
        /// of the operator stack), and finally pushes the result onto the value stack.
        /// </summary>
        /// <param name="operand1"> operand1 represents the first operand to either multiply or divide with the second. </param>
        /// <param name="operand2"> operand2 represents the second operand to either multiply or divide with the first. </param>
        /// <param name="vals"> vals is the value stack. </param>
        /// <param name="operators"> operators is the operator stack. </param>
        /// <exception cref="ArgumentException"> Throws an ArgumentException if a division by zero occurs. </exception>
        private static void MultiplyOrDivide(double operand1, double operand2, Stack<double> vals, Stack<string> operators)
        {
            string op = operators.Pop();
            if (op == "*")
            {
                vals.Push(operand1 * operand2);
            }

            else
            {
                // make sure division by zero does not occur
                if (operand2 == 0.0)
                {
                    throw new ArgumentException();
                }
                vals.Push(operand1 / operand2);
            }
        }


        /// <summary>
        /// This is a private helper method for the Evaluate algorithm that takes in a value stack and an operator stack, 
        /// pops 2 values off the value stack, either adds or subtracts the two values (depending on what is on top 
        /// of the operator stack), and finally pushes the result onto the value stack.
        /// </summary>
        /// <param name="vals"> vals is the value stack. </param>
        /// <param name="operators"> operators is the operators stack. </param>
        private static void AddOrSubtract(Stack<double> vals, Stack<string> operators)
        {
            double val1 = vals.Pop();
            double val2 = vals.Pop();
            string op = operators.Pop();

            if (op == "+")
            {
                vals.Push(val2 + val1);
            }
            else
            {
                vals.Push(val2 - val1);
            }
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// </summary>
        /// <param name="lookup"> Represents the delegate to look up the value of variables with. </param>
        /// <returns> Returns a FormulaError if an undefined variable or division by zero is encountered,
        /// otherwise returns the value of the formula. </returns>
        public object Evaluate(Func<string, double> lookup)
        {
            // these stacks will hold the numerical values and operators of the input expression, respectively
            Stack<double> vals = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            // process each token in order
            foreach (string token in tokens)
            {
                // if the token is a number
                if (Double.TryParse(token, out double result))
                {
                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                    {
                        double val = vals.Pop();

                        // apply the top of the operator stack to the popped value and the token
                        try
                        {
                            MultiplyOrDivide(val, result, vals, operators);
                        }
                        catch (ArgumentException e)
                        {
                            return new FormulaError("A division by zero occurred.");
                        }
                    }

                    else
                    {
                        vals.Push(result);
                    }
                }

                // else if the token is + or -
                else if (token == "+" || token == "-")
                {
                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        // apply the top of the operator stack to the top 2 values of the vals stack
                        AddOrSubtract(vals, operators);
                    }

                    operators.Push(token);
                }


                // else if the token is * or / or (
                else if (token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }


                // else if the token is )
                else if (token == ")")
                {
                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        // apply the top of the operator stack to the top 2 values of the vals stack
                        AddOrSubtract(vals, operators);
                    }

                    // the top of the operators stack should now be a (
                    operators.Pop();

                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                    {
                        double val1 = vals.Pop();
                        double val2 = vals.Pop();

                        // apply the top of the operator stack to the 2 popped values
                        try
                        {
                            MultiplyOrDivide(val2, val1, vals, operators);
                        }
                        catch (ArgumentException e)
                        {
                            return new FormulaError("A division by zero occurred.");
                        }
                    }
                }

                // else (therefore the token is a variable)
                else
                {
                    if (lookup == null)
                    {
                        return new FormulaError("A variable was encountered but no lookup delegate was provided.");
                    }

                    try
                    {
                        double variableValue = lookup(normalizer(token));

                        if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                        {
                            double val = vals.Pop();

                            // apply the top of the operator stack to the popped value and the variable value
                            MultiplyOrDivide(val, variableValue, vals, operators);
                        }
                        else
                        {
                            vals.Push(variableValue);
                        }
                    }
                    catch (ArgumentException e)
                    {
                        return new FormulaError("An undefined variable was encountered.");
                    }
                }
            }

            // when the last token has been processed:

            // if the operator stack is empty
            if (operators.Count == 0)
            {
                return vals.Pop();
            }

            // if the operator stack is not empty
            else
            {
                double val1 = vals.Pop();
                double val2 = vals.Pop();
                string op = operators.Pop();

                // apply the leftover operator to the leftover values and return the result
                if (op == "+")
                {
                    return val2 + val1;
                }
                else
                {
                    return val2 - val1;
                }
            }
        }


        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// </summary>
        /// <returns> An IEnumerable of the variables in this formula without repeats. </returns>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> variables = new HashSet<string>();

            foreach (string token in tokens)
            {
                // if the token is a valid variable and not a duplicate, put the normalized
                // version in the set
                if (isVariable(token) && validator(token))
                {
                    string normalized = normalizer(token);
                    if (!variables.Contains(normalized))
                    {
                        variables.Add(normalized);
                    }
                }
            }

            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// </summary>
        /// <returns> Returns the string representation of this formula. </returns>
        public override string ToString()
        {
            string formulaString = "";
            foreach (string token in tokens)
            {
                // if token is a variable, normalize it
                if (isVariable(token))
                {
                    formulaString += normalizer(token);
                }

                // if it's a number, get it to normalized format
                else if (Double.TryParse(token, out double result))
                {
                    formulaString += result.ToString();
                }

                // otherwise it's just an operator
                else
                {
                    formulaString += token;
                }
            }

            return formulaString;
        }


        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// </summary>
        /// <param name="obj"> the object to compare to this formula </param>
        /// <returns> True if this formula equals the passed in object, false otherwise. </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Formula)
            {
                return false;
            }

            else if (this.ToString() == obj.ToString())
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <param name="f1"> The first formula to compare. </param>
        /// <param name="f2"> The second formula to compare. </param>
        /// <returns> True if f1 == f2, false otherwise. </returns>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <param name="f1"> The first formula to compare. </param>
        /// <param name="f2"> The second formula to compare. </param>
        /// <returns> True if f1 != f2, false otherwise. </returns>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        /// <returns> A hash code for this formula. </returns>
        public override int GetHashCode()
        {
            IEnumerator<string> e = this.tokens.GetEnumerator();
            int code = 0;

            // iterate through each token and use string's GetHashCode method to create an accumulation
            while (e.MoveNext())
            {
                if (Double.TryParse(e.Current, out double result))
                {
                    code += result.ToString().GetHashCode();
                }

                // if token is a variable, make sure it's normalized
                else if (isVariable(e.Current))
                {
                    code += this.normalizer(e.Current).GetHashCode();
                }

                else
                {
                    code += e.Current.GetHashCode();
                }
            }

            return code;
        }

        //private static List<string> Tokenize(string expression)
        //{
        //    var tokens = new List<string>();
        //    var numberBuilder = new StringBuilder();
        //    bool lastWasOperator = true;

        //    for (int i = 0; i < expression.Length; i++)
        //    {
        //        char c = expression[i];

        //        if (char.IsDigit(c) || c == '.')
        //        {
        //            numberBuilder.Append(c);
        //            lastWasOperator = false;
        //        }
        //        else
        //        {
        //            if (numberBuilder.Length > 0)
        //            {
        //                Debug.WriteLine(numberBuilder.ToString());
        //                tokens.Add(numberBuilder.ToString());
        //                numberBuilder.Clear();
        //            }

        //            if (c == '-' && lastWasOperator)
        //            {
        //                // If last was an operator, treat "-" as part of a negative number.
        //                numberBuilder.Append(c);
        //            }
        //            else
        //            {
        //                Debug.WriteLine(c.ToString());
        //                tokens.Add(c.ToString());
        //                lastWasOperator = (c != ')');
        //            }
        //        }
        //    }

        //    if (numberBuilder.Length > 0)
        //    {
        //        Debug.WriteLine(numberBuilder.ToString());
        //        tokens.Add(numberBuilder.ToString());
        //    }

        //    return tokens;

        //}



        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        /// <param name="formula"> The formula to get tokens from. </param>
        /// <returns> An IEnumerable of the passed in formula's tokens. </returns>
        private static IEnumerable<string> Tokenize(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        /// <param name="message"> The explanatory message. </param>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"> The reason for the error. </param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}
