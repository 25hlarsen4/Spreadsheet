//using System.Text.RegularExpressions;

//namespace FormulaEvaluator
//{
//    /// <summary>
//    /// Author:      Hannah Larsen
//    /// Partner:     None
//    /// Date:        13-Jan-2023
//    /// Course:      CS3500, University of Utah, School of Computing
//    /// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
//    /// 
//    /// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
//    /// All references used in the completion of the assignment are cited in my README file.
//    /// 
//    /// File Contents:
//    /// This file contains a library class with an Evaluate method that will evaluate and return the integer result of any
//    /// valid mathematical infix expression using only positive integers and the +, -, *, /, (, and ) operators. Variables 
//    /// may be also be included, and their values will be looked up using a delegate.
//    /// 
//    /// </summary>
//    public static class Evaluator
//    {

//        /// <summary>
//        /// This delegate declaration allows the integer value of variables to be looked up.
//        /// </summary>
//        /// <param name="variable_name"> variable_name represents the variable to look up the value of. </param>
//        /// <returns> Returns the integer value the variable represents. </returns>
//        public delegate int Lookup(String variableName);


//        /// <summary>
//        /// This is a private helper method for the Evaluate algorithm that takes in two integer operands, a value stack, 
//        /// and an operator stack, either multiplies or divides the two operands (depending on what is on top 
//        /// of the operator stack), and finally pushes the result onto the value stack.
//        /// </summary>
//        /// <param name="operand1"> operand1 represents the first operand to either multiply or divide with the second. </param>
//        /// <param name="operand2"> operand2 represents the second operand to either multiply or divide with the first. </param>
//        /// <param name="vals"> vals is the value stack. </param>
//        /// <param name="operators"> operators is the operator stack. </param>
//        /// <exception cref="ArgumentException"></exception>
//        private static void MultiplyOrDivide(int operand1, int operand2, Stack<int> vals, Stack<string> operators)
//        {
//            string op = operators.Pop();
//            if (op == "*")
//            {
//                vals.Push(operand1 * operand2);
//            }
//            else
//            {
//                // make sure division by zero does not occur
//                if (operand2 == 0)
//                {
//                    throw new ArgumentException();
//                }
//                vals.Push(operand1 / operand2);
//            }
//        }


//        /// <summary>
//        /// This is a private helper method for the Evaluate algorithm that takes in a value stack and an operator stack, 
//        /// pops 2 values off the value stack, either adds or subtracts the two values (depending on what is on top 
//        /// of the operator stack), and finally pushes the result onto the value stack.
//        /// </summary>
//        /// <param name="vals"> vals is the value stack. </param>
//        /// <param name="operators"> operators is the operators stack. </param>
//        private static void AddOrSubtract(Stack<int> vals, Stack<string> operators)
//        {
//            int val1 = vals.Pop();
//            int val2 = vals.Pop();
//            string op = operators.Pop();

//            if (op == "+")
//            {
//                vals.Push(val2 + val1);
//            }
//            else
//            {
//                vals.Push(val2 - val1);
//            }
//        }


//        /// <summary>
//        /// This method takes in an infix expression to be evaluated and a delegate to look up the value
//        /// of a variable, evaluates the expression, and returns the result of the evaluation.
//        /// </summary>
//        /// <param name="expression"> expression represents the expression to be evaluated. </param>
//        /// <param name="variableEvaluator"> variableEvaluator represents the delegate to look up the value of any 
//        /// variables that may be in the input expression. </param>
//        /// <returns> Returns the integer the input expression evaluates to. </returns>
//        /// <exception cref="ArgumentException"> Throws an ArgumentException if the input expression is
//        /// invalid or if division by zero occurs. </exception>
//        public static int Evaluate(String expression, Lookup variableEvaluator)
//        {
//            if (expression == null)
//            {
//                throw new ArgumentException();
//            }

//            // split the input expression into separate tokens and get rid of whitespace in each
//            string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
//            for (int i = 0; i < tokens.Length; i++)
//            {
//                tokens[i] = tokens[i].Trim();
//            }

//            // these stacks will hold the numerical values and operators of the input expression, respectively
//            Stack<int> vals = new Stack<int>();
//            Stack<string> operators = new Stack<string>();

//            // process each token in order
//            foreach (string token in tokens)
//            {
//                // if the token is an empty string due to whitespaces in the input expression, ignore it
//                if (token == "")
//                {
//                    continue;
//                }

//                // if the token is an int
//                else if (int.TryParse(token, out int result))
//                {
//                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
//                    {
//                        // make sure the value stack is able to be popped
//                        if (vals.Count == 0)
//                        {
//                            throw new ArgumentException();
//                        }

//                        int val = vals.Pop();

//                        // apply the top of the operator stack to the popped value and the token
//                        MultiplyOrDivide(val, result, vals, operators);
//                    }
//                    else
//                    {
//                        vals.Push(result);
//                    }
//                }

//                // else if the token is a variable
//                else if (Regex.IsMatch(token, "^[a-zA-Z]+[0-9]+$"))
//                {
//                    if (variableEvaluator == null)
//                    {
//                        throw new ArgumentException();
//                    }

//                    // use the delegate to get the value of the variable
//                    int var = variableEvaluator(token);

//                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
//                    {
//                        // make sure the value stack is able to be popped
//                        if (vals.Count == 0)
//                        {
//                            throw new ArgumentException();
//                        }

//                        int val = vals.Pop();

//                        // apply the top of the operator stack to the popped value and the variable value
//                        MultiplyOrDivide(val, var, vals, operators);
//                    }
//                    else
//                    {
//                        vals.Push(var);
//                    }
//                }

//                // else if the token is + or -
//                else if (token == "+" || token == "-")
//                {
//                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
//                    {
//                        // make sure the value stack is able to be popped twice
//                        if (vals.Count < 2)
//                        {
//                            throw new ArgumentException();
//                        }

//                        // apply the top of the operator stack to the top 2 values of the vals stack
//                        AddOrSubtract(vals, operators);
//                    }
//                    operators.Push(token);
//                }


//                // else if the token is * or / or (
//                else if (token == "*" || token == "/" || token == "(")
//                {
//                    operators.Push(token);
//                }


//                // else if the token is )
//                else if (token == ")")
//                {
//                    if (operators.Count > 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
//                    {
//                        // make sure the value stack is able to be popped twice
//                        if (vals.Count < 2)
//                        {
//                            throw new ArgumentException();
//                        }

//                        // apply the top of the operator stack to the top 2 values of the vals stack
//                        AddOrSubtract(vals, operators);
//                    }

//                    // the top of the operators stack should now be a (
//                    if ((operators.Count > 0 && operators.Peek() != "(") || operators.Count == 0)
//                    {
//                        throw new ArgumentException();
//                    }
//                    operators.Pop();

//                    if (operators.Count > 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
//                    {
//                        // make sure the value stack is able to be popped twice
//                        if (vals.Count < 2)
//                        {
//                            throw new ArgumentException();
//                        }

//                        int val1 = vals.Pop();
//                        int val2 = vals.Pop();

//                        // apply the top of the operator stack to the 2 popped values
//                        MultiplyOrDivide(val2, val1, vals, operators);
//                    }
//                }

//                // else (therefore the token is an illegal character)
//                else
//                {
//                    throw new ArgumentException();
//                }

//            }

//            // when the last token has been processed:

//            // if the operator stack is empty
//            if (operators.Count == 0)
//            {
//                // make sure the value stack has only one value in it and return it
//                if (vals.Count != 1)
//                {
//                    throw new ArgumentException();
//                }

//                return vals.Pop();
//            }

//            // if the operator stack is not empty
//            else
//            {
//                // make sure the operator stack has exactly 1 operator left and the value stack has
//                // exactly 2 values left
//                if (operators.Count != 1 || vals.Count != 2)
//                {
//                    throw new ArgumentException();
//                }

//                int val1 = vals.Pop();
//                int val2 = vals.Pop();
//                string op = operators.Pop();

//                // apply the leftover operator to the leftover values and return the result
//                if (op == "+")
//                {
//                    return val2 + val1;
//                }
//                else
//                {
//                    return val2 - val1;
//                }
//            }
//        }
//    }
//}