using System.Collections;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {

        public delegate int Lookup(String variable_name);

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


                // else if token is variable


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
                            vals.Push(val1 + val2);
                        }
                        else
                        {
                            vals.Push(val1 - val2);
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
                            vals.Push(val1 + val2);
                        }
                        else
                        {
                            vals.Push(val1 - val2);
                        }
                    }

                    if (operators.Count > 0 && operators.Peek() != "(")
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
                            vals.Push(val1 * val2);
                        }
                        else
                        {
                            if (val2 == 0)
                            {
                                throw new ArgumentException();
                            }
                            vals.Push(val1 / val2);
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
                    return val1 + val2;
                }
                else
                {
                    return val1 - val2;
                }
            }
        }
    }
}