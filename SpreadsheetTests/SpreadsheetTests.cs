using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void TestSetNewCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            ISet<string> set = sheet.SetCellContents("a1", 2.2);
            Assert.IsTrue(set.Contains("a1"));
        }

        [TestMethod]
        public void TestSetNewCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            ISet<string> set = sheet.SetCellContents("a1", "hi");
            Assert.IsTrue(set.Contains("a1"));
        }

        [TestMethod]
        public void TestSetNewCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            ISet<string> set = sheet.SetCellContents("a1", new Formula("2+2"));
            Assert.IsTrue(set.Contains("a1"));
        }

        [TestMethod]
        public void TestSetSeveralNewCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 2.2);
            sheet.SetCellContents("b1", new Formula("a1+1"));
            ISet<string> set = sheet.SetCellContents("a1", 2.5);
            Assert.IsTrue(set.Contains("a1"));
            Assert.IsTrue(set.Contains("b1"));
        }

        [TestMethod]
        public void TestSetCellContentsCycleCaused()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", new Formula("b1+1"));
            sheet.SetCellContents("b1", new Formula("c1+1"));
            Action a = () => sheet.SetCellContents("c1", new Formula("a1+1"));
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");
        }

        [TestMethod]
        public void TestComplicatedSetCellContents()
        {
            
        }

        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 2);
            sheet.SetCellContents("b1", new Formula("1/a1"));
            sheet.SetCellContents("c1", new Formula("b1 + 1"));
            sheet.SetCellContents("d1", new Formula("a1 - 2"));

            IEnumerable<string> names = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(4, names.Count());
            Assert.IsTrue(names.Contains("a1"));
            Assert.IsTrue(names.Contains("b1"));
            Assert.IsTrue(names.Contains("c1"));
            Assert.IsTrue(names.Contains("d1"));
        }

        [TestMethod]
        public void TestGetCellContentDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 2.5);
            Assert.AreEqual(2.5, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void TestGetCellContentString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", "hi");
            Assert.AreEqual("hi", sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void TestGetCellContentFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("2+2");
            sheet.SetCellContents("a1", form);
            Assert.AreEqual(form, sheet.GetCellContents("a1"));
        }

        [TestMethod]
        public void TestGetCellContentNonexistentName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 2);
            Assert.AreEqual("", sheet.GetCellContents("b1"));
        }

    }
}