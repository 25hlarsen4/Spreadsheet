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

            Stack<int> vals = new Stack<int>();
            Stack<string> operators = new Stack<string>();

            foreach (String token in tokens)
            {
                // if token is int
                if (int.Parse(token) == 1)
                {
                    if (operators.Peek() == "*" || operators.Peek() == "/")
                    {
                        int val = vals.Pop();
                        string op = operators.Pop();
                        if (op == "*")
                        {
                            vals.Push(val * int.Parse(token));
                        }
                        else
                        {
                            vals.Push(val / int.Parse(token));
                        }
                    }

                    else
                    {
                        vals.Push(int.Parse(token));
                    }
                }


                // if token is variable


                // if token is + or -
                if (token == "+" || token == "-")
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
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


                // if token is * or / or (
                if (token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }


                // if token is (
                //if (token == "(")
                //{
                //    operators.Push(token);
                //}


                // if token is )
                if (token == ")")
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
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

                    operators.Pop();

                    if (operators.Peek() == "*" || operators.Peek() == "/")
                    {
                        int val1 = vals.Pop();
                        int val2 = vals.Pop();
                        string op = operators.Pop();

                        if (op == "*")
                        {
                            vals.Push(val1 * val2);
                        }
                        else
                        {
                            vals.Push(val1 / val2);
                        }
                    }
                }

            }

            if (operators.Count == 0)
            {
                return vals.Pop();
            }

            else
            {
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