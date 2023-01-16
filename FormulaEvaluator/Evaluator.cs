/// <summary>
/// Author:    Hannah Larsen
/// Partner:    None
/// Date:
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
/// 
/// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
/// All references used in the completion of the assignment are cited in my README file.
/// 
/// File Contents:
/// 
/// 
/// </summary>

using System.Collections;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// This library class includes a sole method called Evaluate that takes in a string representing a mathematical
    /// expression and a delegate that can look up the value associated with any variables that may be within said
    /// mathematical expression, and returns the integer that the input expression evaluates to.
    /// </summary>
    public static class Evaluator
    {

        /// <summary>
        /// This delegate declaration allows the integer value of variables to be looked up.
        /// </summary>
        /// <param name="variable_name"> variable_name represents the variable to look up the value of. </param>
        /// <returns> Returns the integer value the variable represents. </returns>
        public delegate int Lookup(String variable_name);

        /// <summary>
        /// This method takes in an infix expression to be evaluated and a delegate to look up the value
        /// of a variable, evaluates the expression, and returns the result of the evaluation.
        /// </summary>
        /// <param name="expression"> expression represents the expression to be evaluated. </param>
        /// <param name="variableEvaluator"> variableEvaluator represents the delegate to look up the value of any 
        /// variables that may be in the input expression. </param>
        /// <returns> Returns the integer the input expression evaluates to. </returns>
        /// <exception cref="ArgumentException"> Throws an ArgumentException if the input expression is
        /// invalid or if division by zero occurs. </exception>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].Trim();
            }

            Stack<int> vals = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            foreach (string token in tokens)
            {
                if (token == "")
                {
                    continue;
                }

                // if token is int
                else if (int.TryParse(token, out int result))
                {
                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                    {
                        if (vals.Count == 0)
                        {
                            throw new ArgumentException();
                        }

                        int val = vals.Pop();
                        string op = operators.Pop();
                        if (op == "*")
                        {
                            vals.Push(val * result);
                        }
                        else
                        {
                            if (result == 0)
                            {
                                throw new ArgumentException();
                            }
                            vals.Push(val / result);
                        }
                    }

                    else
                    {
                        vals.Push(result);
                    }
                }


                // else if token is a variable
                else if (Regex.IsMatch(token, "[a-zA-Z]+[0-9]+"))
                {
                    int var = variableEvaluator(token);

                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                    {
                        if (vals.Count == 0)
                        {
                            throw new ArgumentException();
                        }

                        int val = vals.Pop();
                        string op = operators.Pop();
                        if (op == "*")
                        {
                            vals.Push(val * var);
                        }
                        else
                        {
                            if (var == 0)
                            {
                                throw new ArgumentException();
                            }
                            vals.Push(val / var);
                        }
                    }

                    else
                    {
                        vals.Push(var);
                    }
                }


                // else if token is + or -
                else if (token == "+" || token == "-")
                {
                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        if (vals.Count < 2)
                        {
                            throw new ArgumentException();   
                        }

                        int val1 = vals.Pop();
                        int val2 = vals.Pop();
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

                    operators.Push(token);
                }


                // else if token is * or / or (
                else if (token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }


                // if token is (
                //if (token == "(")
                //{
                //    operators.Push(token);
                //}


                // else if token is )
                else if (token == ")")
                {
                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        if (vals.Count < 2)
                        {
                            throw new ArgumentException();
                        }

                        int val1 = vals.Pop();
                        int val2 = vals.Pop();
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

                    if ((operators.Count > 0 && operators.Peek() != "(") || operators.Count == 0)
                    {
                        throw new ArgumentException();
                    }
                    operators.Pop();

                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
                    {
                        if (vals.Count < 2)
                        {
                            throw new ArgumentException();
                        }

                        int val1 = vals.Pop();
                        int val2 = vals.Pop();
                        string op = operators.Pop();

                        if (op == "*")
                        {
                            vals.Push(val2 * val1);
                        }
                        else
                        {
                            if (val2 == 0)
                            {
                                throw new ArgumentException();
                            }
                            vals.Push(val2 / val1);
                        }
                    }
                }

                // else (therefore the token is an illegal character)
                else
                {
                    throw new ArgumentException();
                }

            }


            // when the last token has been processed

            // if the operator stack is empty
            if (operators.Count == 0)
            {
                if (vals.Count != 1)
                {
                    throw new ArgumentException();
                }
                return vals.Pop();
            }

            // if the operator stack is not empty
            else
            {
                if (operators.Count != 1 || vals.Count != 2)
                {
                    throw new ArgumentException();
                }

                int val1 = vals.Pop();
                int val2 = vals.Pop();
                string op = operators.Pop();

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
    }
}