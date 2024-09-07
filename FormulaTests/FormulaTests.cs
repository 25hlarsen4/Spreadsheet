using SpreadsheetUtilities;

namespace FormulaTests
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
    /// This file contains a test class for Formula and is intended to contain all FormulaTests Unit Tests.
    /// It tests all methods in the Formula class.
    /// </summary>
    [TestClass]
    public class FormulaTests
    {

        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula has no tokens.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaNoTokens()
        {
            Action a = () => new Formula("");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula has more closing parenthesis than opening ones.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaMoreClosingParenthesisThanOpen()
        {
            Action a = () => new Formula("2+(3-4))");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This also tests that the formula constructor throws a FormulaFormatException when
        /// the formula has more closing parenthesis than opening ones, and is here for the sake
        /// of code coverage.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaMoreClosingParenthesisThanOpen2()
        {
            Action a = () => new Formula("(3 + 2)) - a2");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// a variable is found to be in an illegal format after being normalized.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectNormalizerInvalidates()
        {
            Action a = () => new Formula("_a1", s => "4", s => true);
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This also tests that the formula constructor throws a FormulaFormatException when
        /// a variable is found to be in an illegal format after being normalized, and is here
        /// for the sake of code coverage.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectNormalizerInvalidates2()
        {
            Action a = () => new Formula("2+a1", s => "4", s => true);
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// a ) is not followed by an operator or )
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaInvalidOrdering()
        {
            Action a = () => new Formula("3 + 2)(");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula does not end in a ), number, or variable.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaInvalidEnding()
        {
            Action a = () => new Formula("3 * 2 -");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula does not start with a (, number, or variable.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormula1InvalidBeginning()
        {
            Action a = () => new Formula("+ 3 * 2");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula includes an invalid variable as determined by the passed in validator.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaInvalidVariable()
        {
            Action a = () => new Formula("3 * 2 - a2", s => s, s => false);
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula includes an invalid variable as determined by the passed in validator.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaIllegalVariable()
        {
            Action a = () => new Formula("3.5 * 2a + 4.5");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This also tests that the formula constructor throws a FormulaFormatException when
        /// the formula includes an invalid variable as determined by the passed in validator, and
        /// is here for the sake of code coverage.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaInvalidVariable2()
        {
            Action a = () => new Formula("a2 * 2", s => s, s => false);
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that the formula constructor throws a FormulaFormatException when
        /// the formula includes an invalid token.
        /// </summary>
        [TestMethod]
        public void TestSyntacticallyIncorrectFormulaInvalidToken()
        {
            Action a = () => new Formula("2 - !");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }


        /// <summary>
        /// This tests that an explicit division by zero returns a FormulaError.
        /// </summary>
        [TestMethod]
        public void TestEvaluateExplicitDivisionByZero()
        {
            Formula form = new Formula("2.02/0");
            Assert.AreEqual(form.Evaluate(null), new FormulaError("A division by zero occurred."));
        }


        /// <summary>
        /// This tests that an implicit division by zero returns a FormulaError.
        /// </summary>
        [TestMethod]
        public void TestEvaluateImplicitDivisionByZero()
        {
            Formula form = new Formula("2 / (4-4)");
            Assert.AreEqual(form.Evaluate(null), new FormulaError("A division by zero occurred."));
        }


        /// <summary>
        /// This tests that the evaluate method can correctly evaluate a series of 
        /// complicated expressions.
        /// </summary>
        [TestMethod]
        public void TestComplicatedEvaluate()
        {
            Formula a = new Formula("30/5/2");
            Formula b = new Formula("2   +(7- 1) / 2");
            Formula c = new Formula("5+10/2-1*3");
            Formula d = new Formula("(3+5+(2*2)*(3*3))");
            Formula e = new Formula("(2+ 5) /  2* (4-1)");
            Formula f = new Formula("((3+5+(2*2)*(3*3)) /2) *3");
            Formula g = new Formula("(((3 - 4) - 2) * 5) / 2");
            Formula h = new Formula("(2+ 5) /  1.5 * (4-1)");

            // test cases with variables
            Formula k = new Formula("2 + (_a1 - (2.5 / _a1))");
            Formula l = new Formula("___*(x2_+(r_3*(t+(_*_3))))");

            Assert.AreEqual(Convert.ToDouble(3), a.Evaluate(null));
            Assert.AreEqual(Convert.ToDouble(5), b.Evaluate(null));
            Assert.AreEqual(Convert.ToDouble(7), c.Evaluate(null));
            Assert.AreEqual(Convert.ToDouble(44), d.Evaluate(null));
            Assert.AreEqual(10.5, e.Evaluate(null));
            Assert.AreEqual(Convert.ToDouble(66), f.Evaluate(null));
            Assert.AreEqual(-7.5, g.Evaluate(null));
            Assert.AreEqual(Convert.ToDouble(14), h.Evaluate(null));
            Assert.AreEqual(2.75, k.Evaluate(s => 2));
            Assert.AreEqual(Convert.ToDouble(3), l.Evaluate(s => 1));
        }


        /// <summary>
        /// This tests that the evaluate method correctly truncates even a 
        /// number that was input into the formula rather than produced by it.
        /// </summary>
        [TestMethod]
        public void TestEvaluateInputTruncates()
        {
            Formula form = new Formula("1.00000000000000002 + 2");
            Assert.AreEqual(Convert.ToDouble(3), form.Evaluate(null));
        }


        /// <summary>
        /// This tests that the evaluate method can handle repeating decimals.
        /// </summary>
        [TestMethod]
        public void TestEvaluateRepeatingDecimals()
        {
            Formula a = new Formula("(2+ 5) /  1.5 * (4-2)");
            Formula b = new Formula("2/3");
            Assert.AreEqual(9.3333333333333333, a.Evaluate(null));
            Assert.AreEqual(0.6666666666666666, b.Evaluate(null));
        }


        /// <summary>
        /// This tests that a negative result can correctly be achieved by the evaulate method.
        /// </summary>
        [TestMethod]
        public void TestEvaluateNegativeResult()
        {
            Formula form = new Formula("( 5-10 )/2");
            Assert.AreEqual(form.Evaluate(null), -2.5);
        }


        /// <summary>
        /// This tests that the evaluate method returns a ForumlaError when a variable
        /// does not have a value when looked up by the lookup delegate.
        /// </summary>
        [TestMethod]
        public void TestEvaluateUndefinedVariable()
        {
            Formula form = new Formula("3 * t2");
            Assert.AreEqual(form.Evaluate((s) => throw new ArgumentException()), new FormulaError("An undefined variable was encountered."));
        }


        /// <summary>
        /// This tests that the evaluate method returns a ForumlaError when a variable
        /// is encountered but no lookup delegate is provided.
        /// </summary>
        [TestMethod]
        public void TestEvaluateUndefinedVariable2()
        {
            Formula form = new Formula("3 * t2");
            Assert.AreEqual(form.Evaluate(null), new FormulaError("A variable was encountered but no lookup delegate was provided."));
        }


        /// <summary>
        /// This tests that the evaluate method can correctly look up a variable's
        /// value via the lookup delegate.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDefinedVariable()
        {
            Formula form = new Formula("3 * t2");
            Assert.AreEqual(form.Evaluate((s) => 5), Convert.ToDouble(15));
        }


        /// <summary>
        /// This tests the GetVariables method in a simple case where the identity
        /// normalizer is used.
        /// </summary>
        [TestMethod]
        public void TestGetVariablesIdentityNormalizer()
        {
            Formula form = new Formula("(x + Y) * 2 - z4");
            IEnumerable<string> e = form.GetVariables();
            Assert.AreEqual(3, e.Count());
            Assert.IsTrue(e.Contains("x"));
            Assert.IsTrue(e.Contains("Y"));
            Assert.IsTrue(e.Contains("z4"));
        }


        /// <summary>
        /// This tests the GetVariables method does not return duplicate variables.
        /// </summary>
        [TestMethod]
        public void TestGetVariablesWithDuplicateVariables()
        {
            Formula form = new Formula("(x + Y) * 2 - z4 + x");
            IEnumerable<string> e = form.GetVariables();
            Assert.AreEqual(3, e.Count());
            Assert.IsTrue(e.Contains("x"));
            Assert.IsTrue(e.Contains("Y"));
            Assert.IsTrue(e.Contains("z4"));
        }


        /// <summary>
        /// This tests the ToString method in a simple case where the identity
        /// normalizer is used.
        /// </summary>
        [TestMethod]
        public void TestToStringIdentityNormalizer()
        {
            Formula form = new Formula("x + Y");
            Assert.AreEqual("x+Y", form.ToString());
        }


        /// <summary>
        /// This tests the ToString method in a case where both doubles and variables
        /// are included in the formula
        /// </summary>
        [TestMethod]
        public void TestToStringNumsAndVariables()
        {
            Formula form = new Formula("(2.00 - x) + Y");
            Assert.AreEqual("(2-x)+Y", form.ToString());
        }


        /// <summary>
        /// This tests the ToString method in a case where the normalizer turns
        /// all variables to upper case.
        /// </summary>
        [TestMethod]
        public void TestToStringInputNormalizer()
        {
            Formula form = new Formula("(2.020 -  t) / d4", s => s.ToUpper(), s => true);
            Assert.AreEqual("(2.02-T)/D4", form.ToString());
        }


        /// <summary>
        /// This tests the Equals method in a case where two doubles are equal
        /// but have a different number of zeros on the end.
        /// </summary>
        [TestMethod]
        public void TestEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsTrue(form1.Equals(form2));
        }


        /// <summary>
        /// This tests the Equals method in a case where a difference in variable
        /// capitalization determines that 2 formulas are not equal.
        /// </summary>
        [TestMethod]
        public void TestEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsFalse(form1.Equals(form2));
        }


        /// <summary>
        /// This tests the Equals method in a case where the passed in variable 
        /// normalizer makes it so 2 formulas are equal, otherwise they would not be.
        /// </summary>
        [TestMethod]
        public void TestEqualsNormalizerMakesTheDifference()
        {
            Formula form1 = new Formula("(2.00 - x) + Y", s => s.ToUpper(), s => true);
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsTrue(form1.Equals(form2));
        }


        /// <summary>
        /// This tests the Equals method in a case where the passed in object is
        /// not of the Formula type.
        /// </summary>
        [TestMethod]
        public void TestEqualsObjNotFormula()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            string obj = "hi";
            Assert.IsFalse(form1.Equals(obj));
        }


        /// <summary>
        /// This tests the Equals method in the case where the passed in object is null.
        /// </summary>
        [TestMethod]
        public void TestEqualsNullObj()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula obj = null;
            Assert.IsFalse(form1.Equals(obj));
        }


        /// <summary>
        /// This tests the == method in the case where the result is true.
        /// </summary>
        [TestMethod]
        public void TestOperatorEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsTrue(form1 == form2);
        }


        /// <summary>
        /// This tests the == method in the case where the result is false.
        /// </summary>
        [TestMethod]
        public void TestOperatorEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsFalse(form1 == form2);
        }


        /// <summary>
        /// This tests the != method in the case where the result is true.
        /// </summary>
        [TestMethod]
        public void TestOperatorNotEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsTrue(form1 != form2);
        }


        /// <summary>
        /// This tests the != method in the case where the result is false.
        /// </summary>
        [TestMethod]
        public void TestOperatorNotEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsFalse(form1 != form2);
        }


        /// <summary>
        /// This tests that the GetHashCode method returns the same hash code
        /// when called on two equal formulas as defined by the equals method.
        /// </summary>
        [TestMethod]
        public void TestGetHashCodeOnTwoEqualFormulas()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            int code1 = form1.GetHashCode();
            int code2 = form2.GetHashCode();
            Assert.AreEqual(code1, code2);
        }


        /// <summary>
        /// This tests that the GetHashCode method returns different hash codes
        /// when called on two unequal formulas as defined by the equals method
        /// (note that this result is not guaranteed for all unequal formulas, but 
        /// should be the case in almost all cases).
        /// </summary>
        [TestMethod]
        public void TestGetHashCodeOnTwoUnequalFormulas()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            int code1 = form1.GetHashCode();
            int code2 = form2.GetHashCode();
            Assert.AreNotEqual(code1, code2);
        }
    }
}