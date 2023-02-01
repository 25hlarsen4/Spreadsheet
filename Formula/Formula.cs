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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Author:      Hannah Larsen
    /// Partner:     None
    /// Date:        20-Jan-2023
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

        // "^[a-zA-Z_]{1}[a-zA-Z_]*$"
        /// <summary>
        /// This is a helper method to be called in the Formula constructors that will throw 
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

            //tokens.MoveNext();       **** I dont think you need this bc the if statement already called it?? *****
            string prevToken = formTokens.Current;

            // the first token must be a (, number, or variable
            if ((prevToken != "(") && !Double.TryParse(prevToken, out double result) && !isVariable(prevToken))
            {
                throw new FormulaFormatException("Formulas must start with either a number, a variable, or a (. Make sure that the formula starts with one of these things.");
            }

            // now check that if it's a variable, it is also valid by the validator
            else if (isVariable(prevToken))
            {
                if (!isVariable(normalizer(prevToken)))
                {
                    throw new FormulaFormatException("A normalized variable was found to be illegal. Make sure the normalized versions of your variables fits the pattern of a letter or underscore followed by zero or more letters, underscores, or digits.");
                }

                if (!validator(normalizer(prevToken)))
                {
                    throw new FormulaFormatException("A normalized variable was found to be invalid. Make sure the normalized versions of your variables meet the conditions of your validator.");
                }
            }

            if (prevToken == "(")
            {
                numOfLeftParenth += 1;
            }

            while (formTokens.MoveNext())
            {
                // i think invalid token will already be found through the below process, but maybe not

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

                    // if current was a variable, make sure it's valid
                    else if (isVariable(formTokens.Current))
                    {
                        if (!isVariable(normalizer(formTokens.Current)))
                        {
                            throw new FormulaFormatException("A normalized variable was found to be illegal. Make sure the normalized versions of your variables fits the pattern of a letter or underscore followed by zero or more letters, underscores, or digits.");
                        }

                        if (!validator(normalizer(formTokens.Current)))
                        {
                            throw new FormulaFormatException("A normalized variable was found to be invalid. Make sure the normalized versions of your variables meet the conditions of your validator.");
                        }
                    }

                    else if (formTokens.Current == "(")
                    {
                        numOfLeftParenth += 1;
                    }
                }

                // if previous is number, variable, or )    (i think at this point we already know a variable would be valid)
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

            // prevToken should now hold the last token, so check if it's valid       (again i think we already know a variable would be valid)
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
        /// This is a helper method that determines if the input string is a 
        /// variable by determining if it matches the variable regular expression.
        /// </summary>
        /// <param name="token"> The string to see if is a variable. </param>
        /// <returns> Returns true if the input string is a variable, and false
        /// otherwise. </returns>
        private static Boolean isVariable(String token)
        {
            if (Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
            tokens = GetTokens(formula);
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
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            normalizer = normalize;
            validator = isValid;

            tokens = GetTokens(formula);
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
        /// <exception cref="ArgumentException"></exception>
        private static void MultiplyOrDivide(double operand1, double operand2, Stack<double> vals, Stack<string> operators)
        {
            string op = operators.Pop();
            if (op == "*")
            {
                vals.Push(operand1 * operand2);
            }
            else
            {
                // are we supposed to be doing float div???
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
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        /// ******************** lookup is a delegate that takes in a string and returns a double ********************
        public object Evaluate(Func<string, double> lookup)
        {
            //if (expression == null)
            //{
            //    throw new ArgumentException();
            //}

            // do I need to do this part????
            //for (int i = 0; i < tokens.Count; i++)
            //{
            //    tokens[i] = tokens[i].Trim();
            //}

            // these stacks will hold the numerical values and operators of the input expression, respectively
            Stack<double> vals = new Stack<double>();
            Stack<string> operators = new Stack<string>();

            // process each token in order
            foreach (string token in tokens)
            {
                // if the token is an empty string due to whitespaces in the input expression, ignore it
                //if (token == "")
                //{
                //    continue;
                //}

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
                        } catch (ArgumentException e)
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
                    // do i need to check for this????
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
                    } catch (ArgumentException e)
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
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> variables = new HashSet<string>();

            foreach (string token in tokens)
            {
                if (Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*") && validator(token))
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
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string formulaString = "";
            foreach (string token in tokens)
            {
                // is it possible for there to be an empty string???
                // do i need to worry about variables being valid???

                // if token is a variable, normalize it
                if (Regex.IsMatch(token, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
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
        ///  <change> make object nullable </change>
        ///
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
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Formula)
            {
                return false;
            }

            //formula? form = obj as formula;
            //Formula? form = (Formula)obj;
            //formula form = (formula)obj;

            else if (this.ToString() == obj.ToString())
            {
                return true;
            }

            else
            {
                return false;
            }

            //// if each formula has a different number of tokens, we know they're not equal
            //if (this.tokens.Count() != form.tokens.Count())
            //{
            //    return false;
            //}

            //// now we know they have the same number of tokens
            //// convert the tokens into lists so they can be indexable
            //List<string> tokens1 = this.tokens.ToList();
            //List<string> tokens2 = form.tokens.ToList();

            //for (int i = 0; i < tokens1.Count(); i++)
            //{
            //    // all i need to do?

            //    // if the token is a number
            //    if (double.TryParse(tokens1[i], out double result))
            //    {
            //        if (!double.TryParse(tokens2[i], out double result2))
            //        {
            //            return false;
            //        }

            //        // now we know both tokens are numbers
            //        else if (result.ToString() != result2.ToString())
            //        {
            //            return false;
            //        }
            //    }

            //    // if the token is a variable
            //    else if (Regex.IsMatch(tokens1[i], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            //    {
            //        if (!Regex.IsMatch(tokens2[i], @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            //        {
            //            return false;
            //        }

            //        // now we know both tokens are variables
            //        else if (this.normalizer(tokens1[i]) != form.normalizer(tokens2[i]))
            //        {
            //            return false;
            //        }
            //    }

            //    // otherwise the token is just an operator
            //    else if (tokens1[i] != tokens2[i])
            //    {
            //        return false;
            //    }
            //}

            //return true;
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            IEnumerator<string> e = this.tokens.GetEnumerator();
            int code = 0;
            while (e.MoveNext())
            {
                if (Double.TryParse(e.Current, out double result))
                {
                    code += result.ToString().GetHashCode();
                }

                else if (Regex.IsMatch(e.Current, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
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

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
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
        /// <param name="reason"></param>
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


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
