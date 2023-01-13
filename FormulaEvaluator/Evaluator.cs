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
                // if int
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


                    
            }
        }


    }
}