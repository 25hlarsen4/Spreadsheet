using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestInitialize]
        public void TestInitialize()
        {

        }


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


        [TestMethod]
        public void TestToStringIdentityNormalizer()
        {
            Formula form = new Formula("x + Y");
            Assert.AreEqual("x+Y", form.ToString());
        }


        [TestMethod]
        public void TestToStringNumsAndVariables()
        {
            Formula form = new Formula("(2.00 - x) + Y");
            Assert.AreEqual("(2.00-x)+Y", form.ToString());
        }


        [TestMethod]
        public void TestToStringInputNormalizer()
        {
        }


        [TestMethod]
        public void TestEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsTrue(form1.Equals(form2));
        }


        [TestMethod]
        public void TestEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsFalse(form1.Equals(form2));
        }


        [TestMethod]
        public void TestOperatorEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsTrue(form1 == form2);
        }


        [TestMethod]
        public void TestOperatorEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsFalse(form1 == form2);
        }


        [TestMethod]
        public void TestOperatorNotEqualsTrue()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.00 - X) + Y");
            Assert.IsTrue(form1 != form2);
        }


        [TestMethod]
        public void TestOperatorNotEqualsFalse()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            Assert.IsFalse(form1 != form2);
        }


        [TestMethod]
        public void TestGetHashCodeOnTwoEqualFormulas()
        {
            Formula form1 = new Formula("(2.00 - x) + Y");
            Formula form2 = new Formula("(2.0000 - x  ) + Y");
            int code1 = form1.GetHashCode();
            int code2 = form2.GetHashCode();
            Assert.AreEqual(code1, code2);
        }


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