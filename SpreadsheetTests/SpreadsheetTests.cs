using SpreadsheetUtilities;
using SS;
using System.Xml;

namespace SpreadsheetTests
{
    /// <summary> aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
    /// Author:      Hannah Larsen
    /// Partner:     None
    /// Date:        03-Feb-2023
    /// Course:      CS3500, University of Utah, School of Computing
    /// Copyright:   CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
    /// 
    /// I, Hannah Larsen, certify that I wrote this code from scratch and did not copy it in part or whole from another source.
    /// All references used in the completion of the assignment are cited in my README file.
    /// 
    /// File Contents:
    /// This file contains a test class for the Spreadsheet class and is intended to contain all SpreadsheetTests Unit Tests.
    /// It tests all methods in the Spreadsheet class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// This tests the save method and the 4-argument spreadsheet constructor 
        /// that constructs a spreadsheet based off an input xml file.
        /// </summary>
        [TestMethod]
        public void TestSaveAndLoadSpreadsheet()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.SetContentsOfCell("c1", "hi");
            Assert.IsTrue(sheet.Changed);
            sheet.Save("save.txt");
            Assert.IsFalse(sheet.Changed);

            Spreadsheet sheet2 = new Spreadsheet("save.txt", s => true, s => s, "default");
            Assert.AreEqual(Convert.ToDouble(2), sheet2.GetCellValue("a1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet2.GetCellValue("b1"));
            Assert.AreEqual("hi", sheet2.GetCellContents("c1"));
        }

        /// <summary>
        /// This also tests the save method and the 4-argument spreadsheet constructor 
        /// that constructs a spreadsheet based off an input xml file, where one 
        /// of the cell values in the newly constructed spreadsheet is a FormulaError
        /// </summary>
        [TestMethod]
        public void TestSaveAndLoadSpreadsheetWithFormulaError()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=c1+1");
            sheet.SetContentsOfCell("c1", "hi");
            sheet.Save("save5.txt");

            Spreadsheet sheet2 = new Spreadsheet("save5.txt", s => true, s => s, "default");
            Assert.AreEqual(Convert.ToDouble(2), sheet2.GetCellValue("a1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet2.GetCellValue("b1"));
            Assert.AreEqual("hi", sheet2.GetCellValue("c1"));
        }

        /// <summary>
        /// This tests the save method and the 4-argument spreadsheet constructor 
        /// that constructs a spreadsheet based off an input xml file in a complicated
        /// case where one of the cells has been reset.
        /// </summary>
        [TestMethod]
        public void TestComplicatedSaveAndLoadSpreadsheet()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=c1+1");
            sheet.SetContentsOfCell("c1", "hi");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.Save("save6.txt");

            Spreadsheet sheet2 = new Spreadsheet("save6.txt", s => true, s => s, "default");
            Assert.AreEqual(Convert.ToDouble(2), sheet2.GetCellValue("a1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet2.GetCellValue("b1"));
            Assert.AreEqual("hi", sheet2.GetCellValue("c1"));
        }

        /// <summary>
        /// This tests that the Save method throws a SpreadsheetReadWriteException 
        /// if the filepath is invalid.
        /// </summary>
        [TestMethod]
        public void TestSaveException()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.Save("/some/nonsense/path.xml");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the 4-argument spreadsheet constructor throws a 
        /// SpreadsheetReadWriteException when a nonexistent xml file is passed in.
        /// </summary>
        [TestMethod]
        public void TestLoadSpreadsheetFileNotFound()
        {
            Action a = () => new Spreadsheet("nonexistent.txt", s => true, s => s, "default");
            Assert.ThrowsException<SpreadsheetReadWriteException> (a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the 4-argument spreadsheet constructor throws a 
        /// SpreadsheetReadWriteException when during the construction, an 
        /// invalid name is encountered.
        /// </summary>
        [TestMethod]
        public void TestLoadSpreadsheetInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.SetContentsOfCell("c1", "hi");
            sheet.Save("save4.txt");

            Action a = () => new Spreadsheet("save4.txt", s => false, s => s, "default");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the 4-argument spreadsheet constructor throws a 
        /// SpreadsheetReadWriteException if the version passed to the constructor
        /// and the version in the xml file do not match.
        /// </summary>
        [TestMethod]
        public void TestLoadSpreadsheetVersionsDontMatch()
        { 
            // this spreadsheet's version should be "default"
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.Save("save2.txt");

            // "default" and "1" don't match, so a SpreadsheetReadWriteException should be thrown
            Action a = () => new Spreadsheet("save2.txt", s => true, s => s, "1");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the 4-argument spreadsheet constructor throws a 
        /// SpreadsheetReadWriteException if during the construction, it encounters
        /// a circular dependency.
        /// </summary>
        [TestMethod]
        public void TestLoadSpreadsheetCircularException()
        {
            using (XmlWriter writer = XmlWriter.Create("circ.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "a1");
                writer.WriteElementString("contents", "=b1");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "b1");
                writer.WriteElementString("contents", "=a1");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Action a = () => new Spreadsheet("circ.txt", s => true, s => s, "default");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the 4-argument spreadsheet constructor throws a 
        /// SpreadsheetReadWriteException if during the construction, it encounters
        /// an invalid Formula.
        /// </summary>
        [TestMethod]
        public void TestLoadSpreadsheetFormulaFormatException()
        {
            using (XmlWriter writer = XmlWriter.Create("format.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "default");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "a1");
                writer.WriteElementString("contents", "=1++1");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Action a = () => new Spreadsheet("format.txt", s => true, s => s, "default");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests the GetSavedVersion method in a simple case.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersion()
        {
            // this spreadsheet's version should be "default"
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.Save("save3.txt");

            Assert.AreEqual("default", sheet.GetSavedVersion("save3.txt"));
        }

        /// <summary>
        /// This tests that the GetSavedVersion method throws a 
        /// SpreadsheetReadWriteException if the passed in xml file does not exist.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersionNonExistentFile()
        {
            Spreadsheet sheet = new Spreadsheet();

            Action a = () => sheet.GetSavedVersion("nonexistent.txt");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the GetSavedVersion method throws a 
        /// SpreadsheetReadWriteException if there is no version in the xml file representing
        /// a spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersionNoVersion()
        {
            using (XmlWriter writer = XmlWriter.Create("novers.txt"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "a1");
                writer.WriteElementString("contents", "1");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Spreadsheet sheet = new Spreadsheet();

            Action a = () => sheet.GetSavedVersion("novers.txt");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the GetSavedVersion method throws a 
        /// SpreadsheetReadWriteException if the xml file passed in is not in the
        /// required form and therefore no version is found.
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersionNoVersion2()
        {
            using (XmlWriter writer = XmlWriter.Create("novers2.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "a1");
                writer.WriteElementString("contents", "1");
                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            Spreadsheet sheet = new Spreadsheet();

            Action a = () => sheet.GetSavedVersion("novers2.txt");
            Assert.ThrowsException<SpreadsheetReadWriteException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests the GetCellValue method in the cases where the contents
        /// are a double and a Formula.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueFormula()
        {
            //Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            Assert.AreEqual(Convert.ToDouble(2), sheet.GetCellValue("a1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet.GetCellValue("b1"));
        }

        /// <summary>
        /// This tests the GetCellValue method in the cases where the contents
        /// is a string.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueString()
        {
            //Spreadsheet sheet = new Spreadsheet(s => true, s => s, "default");
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "hi");
            Assert.AreEqual("hi", sheet.GetCellValue("a1"));
        }

        /// <summary>
        /// This tests the GetCellValue method in the case where there are nested
        /// formulas.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueNestedFormulas()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+1");
            sheet.SetContentsOfCell("c1", "=b1+1");
            Assert.AreEqual(Convert.ToDouble(2), sheet.GetCellValue("a1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet.GetCellValue("b1"));
            Assert.AreEqual(Convert.ToDouble(4), sheet.GetCellValue("c1"));
        }

        /// <summary>
        /// This tests the GetCellValue method in the case where there are nested
        /// formulas and several evaluate to FormulaErrors.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueNestedFormulaError()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1/0");
            sheet.SetContentsOfCell("c1", "=b1+1");
            Assert.AreEqual(Convert.ToDouble(2), sheet.GetCellValue("a1"));
            Assert.AreEqual(new FormulaError("A division by zero occurred."), sheet.GetCellValue("b1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("c1"));
        }

        /// <summary>
        /// This tests the GetCellValue method in the case where the lookup
        /// delegate throws.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueWithEmptyVariable()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("a1"));
        }

        /// <summary>
        /// This tests the GetCellValue method in the case where one value
        /// is a Formula Error.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueFormulaError()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "hi");
            sheet.SetContentsOfCell("b1", "=a1+1");
            Assert.AreEqual("hi", sheet.GetCellValue("a1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("b1"));
        }

        /// <summary>
        /// This tests that the GetCellValue method returns an empty string
        /// when called on an empty cell.
        /// </summary>
        [TestMethod]
        public void TestGetCellValueOfEmptyCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.AreEqual("", sheet.GetCellValue("a1"));
        }

        /// <summary>
        /// This tests the SetCellContents method where the cell contents are
        /// set to a double.
        /// </summary>
        [TestMethod]
        public void TestSetNewCellContentsDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            IList<string> set = sheet.SetContentsOfCell("a1", "2.2");
            Assert.IsTrue(set.Contains("a1"));
        }

        /// <summary>
        /// This tests the SetCellContents method where the cell contents are
        /// set to a string.
        /// </summary>
        [TestMethod]
        public void TestSetNewCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            IList<string> set = sheet.SetContentsOfCell("a1", "hi");
            Assert.IsTrue(set.Contains("a1"));
        }

        /// <summary>
        /// This tests the SetCellContents method in the case that a previously
        /// nonempty cell is set to be empty, ie is set to contain the empty string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsToEmptyString()
        {
            Spreadsheet sheet = new Spreadsheet();

            IList<string> aset = sheet.SetContentsOfCell("a1", "=b1+1");
            Assert.IsTrue(aset.Contains("a1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("a1"));

            IList<string> bset = sheet.SetContentsOfCell("b1", "2");
            Assert.IsTrue(bset.Contains("b1"));
            Assert.IsTrue(bset.Contains("a1"));
            Assert.AreEqual(Convert.ToDouble(2), sheet.GetCellValue("b1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet.GetCellValue("a1"));

            // make sure that both a1 and b1 are considered nonempty
            IEnumerable<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(2, nonEmptyCells.Count());
            Assert.IsTrue(nonEmptyCells.Contains("a1"));
            Assert.IsTrue(nonEmptyCells.Contains("b1"));

            // make b1 empty
            IList<string> bNewSet = sheet.SetContentsOfCell("b1", "");
            Assert.IsTrue(bNewSet.Contains("b1"));
            Assert.IsTrue(bNewSet.Contains("a1"));
            Assert.AreEqual("", sheet.GetCellValue("b1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("a1"));

            // make sure that only a1 is considered nonempty now
            IEnumerable<string> newNonEmptyCells = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(1, newNonEmptyCells.Count());
            Assert.IsTrue(newNonEmptyCells.Contains("a1"));
        }

        /// <summary>
        /// This tests the SetCellContents method where the cell contents are
        /// set to a Formula.
        /// </summary>
        [TestMethod]
        public void TestSetNewCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            IList<string> set = sheet.SetContentsOfCell("a1", "=2+2");
            Assert.IsTrue(set.Contains("a1"));
            Assert.AreEqual(Convert.ToDouble(4), sheet.GetCellValue("a1"));
        }

        /// <summary>
        /// This tests the REsetting a cell's contents, ie. a case in which 
        /// a nonempty cell is reset to have new contents.
        /// </summary>
        [TestMethod]
        public void TestSetSeveralNewCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2.2");
            Assert.AreEqual(2.2, sheet.GetCellValue("a1"));
            sheet.SetContentsOfCell("b1", "=a1+1");
            Assert.AreEqual(3.2, sheet.GetCellValue("b1"));

            IList<string> set = sheet.SetContentsOfCell("a1", "2.5");
            Assert.IsTrue(set.Contains("a1"));
            Assert.IsTrue(set.Contains("b1"));
            Assert.AreEqual(2.5, sheet.GetCellValue("a1"));
            Assert.AreEqual(3.5, sheet.GetCellValue("b1"));
        }

        /// <summary>
        /// This tests that the SetContentsOfCell method throws an 
        /// InvalidNameException when it encounters an invalid name.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsInvalidNameByValidator()
        {
            Spreadsheet sheet = new Spreadsheet(s => false, s => s, "default");
            Action a = () => sheet.SetContentsOfCell("a1", "2.2");
            Assert.ThrowsException<InvalidNameException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetCellContents method can detect a cycle in a 
        /// simple direct cycle case.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsDirectCycle()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.SetContentsOfCell("a1", "=a1+1");
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetCellContents method can detect a cycle in a 
        /// case where it occurs indirectly
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsIndirectCycle()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "=b1+1");
            sheet.SetContentsOfCell("b1", "=c1+1");
            Action a = () => sheet.SetContentsOfCell("c1", "=a1+1");
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetCellContents method can detect a cycle in a 
        /// case where in a formula with several variables, only one of the variables is
        /// involved in a cycle.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsComplicatedCycle()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=a1+c1");
            Action a = () => sheet.SetContentsOfCell("c1", "=b1");
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetCellContents method can detect a cycle in a case
        /// where there are 4 cells with initially no cycles between them, but then 
        /// one cell gets reset and creates a cycle.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsMoreComplicatedCycle()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set a1 to a double, it should have no dependents
            IList<string> aSet = sheet.SetContentsOfCell("a1", "2.5");
            Assert.AreEqual(1, aSet.Count);
            Assert.IsTrue(aSet.Contains("a1"));

            // set b1 to a Formula, it should have no dependents
            IList<string> bSet = sheet.SetContentsOfCell("b1", "=a1+1");
            Assert.AreEqual(1, bSet.Count);
            Assert.IsTrue(bSet.Contains("b1"));

            // set c1 to a Formula, it should have no dependents
            IList<string> cSet = sheet.SetContentsOfCell("c1", "=d1+1");
            Assert.AreEqual(1, cSet.Count);
            Assert.IsTrue(cSet.Contains("c1"));

            // set d1 to a Formula, its dependent should be c1
            IList<string> dSet = sheet.SetContentsOfCell("d1", "=b1+1");
            Assert.AreEqual(2, dSet.Count);
            Assert.IsTrue(dSet.Contains("d1"));
            Assert.IsTrue(dSet.Contains("c1"));

            // reset b1 to a Formula that depends on c1, therefore a cycle should be caused
            Action a = () => sheet.SetContentsOfCell("b1", "=c1");
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetCellContents method can detect cycles in several 
        /// different scenarios.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsCycleCausedInSeveralWays()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set a1, it should have no dependents
            IList<string> aSet = sheet.SetContentsOfCell("a1", "=b1+c1");
            Assert.AreEqual(1, aSet.Count);
            Assert.IsTrue(aSet.Contains("a1"));

            // set c1, its dependent should be a1
            IList<string> cSet = sheet.SetContentsOfCell("c1", "=b1");
            Assert.AreEqual(2, cSet.Count);
            Assert.IsTrue(cSet.Contains("c1"));
            Assert.IsTrue(cSet.Contains("a1"));

            // set b1, its dependents should be a1 and c1
            IList<string> bSet = sheet.SetContentsOfCell("b1", "=d1");
            Assert.AreEqual(3, bSet.Count);
            Assert.IsTrue(bSet.Contains("b1"));
            Assert.IsTrue(bSet.Contains("a1"));
            Assert.IsTrue(bSet.Contains("c1"));

            Action a = () => sheet.SetContentsOfCell("d1", "=e1+b1");
            Assert.ThrowsException<CircularException>(a, "failed to throw exception");

            Action b = () => sheet.SetContentsOfCell("d1", "=e1+c1");
            Assert.ThrowsException<CircularException>(b, "failed to throw exception");

            Action c = () => sheet.SetContentsOfCell("d1", "=e1+a1");
            Assert.ThrowsException<CircularException>(c, "failed to throw exception");

            Action d = () => sheet.SetContentsOfCell("d1", "=a1+b1+c1");
            Assert.ThrowsException<CircularException>(d, "failed to throw exception");
        }

        /// <summary>
        /// This tests the SetCellContents method in a complicated case where there
        /// are multiple dependencies between 4 cells.
        /// </summary>
        [TestMethod]
        public void TestComplicatedSetCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set a1 to a double, it should have no dependents
            IList<string> aSet = sheet.SetContentsOfCell("a1", "2.5");
            Assert.AreEqual(1, aSet.Count);
            Assert.IsTrue(aSet.Contains("a1"));

            // set b1 to a Formula, it should have no dependents
            IList<string> bSet = sheet.SetContentsOfCell("b1", "=a1+1");
            Assert.AreEqual(1, bSet.Count);
            Assert.IsTrue(bSet.Contains("b1"));

            // set c1 to a Formula, it should have no dependents
            IList<string> cSet = sheet.SetContentsOfCell("c1", "=d1+1");
            Assert.AreEqual(1, cSet.Count);
            Assert.IsTrue(cSet.Contains("c1"));
            Assert.AreEqual(new FormulaError("An undefined variable was encountered."), sheet.GetCellValue("c1"));

            // set d1 to a Formula, its dependent should be c1
            IList<string> dSet = sheet.SetContentsOfCell("d1", "=b1+1");
            Assert.AreEqual(2, dSet.Count);
            Assert.AreEqual("d1", dSet[0]);
            Assert.AreEqual("c1", dSet[1]);
            Assert.AreEqual(4.5, sheet.GetCellValue("d1"));
            Assert.AreEqual(5.5, sheet.GetCellValue("c1"));

            // reset b1 to a double, its dependents should now be d1 and c1
            IList<string> bSetNew = sheet.SetContentsOfCell("b1", "2");
            Assert.AreEqual(3, bSetNew.Count);
            Assert.AreEqual("b1", bSetNew[0]);
            Assert.AreEqual("d1", bSetNew[1]);
            Assert.AreEqual("c1", bSetNew[2]);
            Assert.AreEqual(Convert.ToDouble(2), sheet.GetCellValue("b1"));
            Assert.AreEqual(Convert.ToDouble(3), sheet.GetCellValue("d1"));
            Assert.AreEqual(Convert.ToDouble(4), sheet.GetCellValue("c1"));
        }

        /// <summary>
        /// This tests the SetCellContents method in a case where a Cell that
        /// previously contained a Formula is reset to contain a new Formula.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsReplaceOldFormWithNewForm()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set a1 to a Formula, it should have no dependents
            IList<string> aSet = sheet.SetContentsOfCell("a1", "=b1+1");
            Assert.AreEqual(1, aSet.Count);
            Assert.IsTrue(aSet.Contains("a1"));

            // set b1 to a double, its dependent should be a1
            IList<string> bSet = sheet.SetContentsOfCell("b1", "2");
            Assert.AreEqual(2, bSet.Count);
            Assert.IsTrue(bSet.Contains("b1"));
            Assert.IsTrue(bSet.Contains("a1"));

            // set c1 to a Formula, it should have no dependents
            IList<string> cSet = sheet.SetContentsOfCell("c1", "=a1");
            Assert.AreEqual(1, cSet.Count);

            // reset a1 to contain new Formula, its dependent should now be c1
            IList<string> aSetNew = sheet.SetContentsOfCell("a1", "=d1+1");
            Assert.AreEqual(2, aSetNew.Count);
            Assert.IsTrue(aSetNew.Contains("a1"));
            Assert.IsTrue(aSetNew.Contains("c1"));
        }

        /// <summary>
        /// This tests the SetCellContents method in a case where a Cell that
        /// previously contained a Formula is reset to contain a string.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentsReplaceOldFormWithNewText()
        {
            Spreadsheet sheet = new Spreadsheet();

            // set a1 to contain a formula, it should have no dependents
            IList<string> aSet = sheet.SetContentsOfCell("a1", "=b1+1");
            Assert.AreEqual(1, aSet.Count);
            Assert.IsTrue(aSet.Contains("a1"));

            // set b1 to contain a double, its dependent should be a1
            IList<string> bSet = sheet.SetContentsOfCell("b1", "2");
            Assert.AreEqual(2, bSet.Count);
            Assert.IsTrue(bSet.Contains("b1"));
            Assert.IsTrue(bSet.Contains("a1"));

            // set c1 to contain a formula, it should have no dependents
            IList<string> cSet = sheet.SetContentsOfCell("c1", "=a1");
            Assert.AreEqual(1, cSet.Count);

            // reset a1 to now contain a string, its dependent should now be c1
            IList<string> aSetNew = sheet.SetContentsOfCell("a1", "hi");
            Assert.AreEqual(2, aSetNew.Count);
            Assert.IsTrue(aSetNew.Contains("a1"));
            Assert.IsTrue(aSetNew.Contains("c1"));
        }

        /// <summary>
        /// This tests that the SetCellContents method throws an InvalidNameException
        /// when an invalid cell name is given.
        /// </summary>
        [TestMethod]
        public void TestSetCellContentInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.SetContentsOfCell("1a", "2");
            Assert.ThrowsException<InvalidNameException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the SetContentsOfCell method throws a FormulaFormatException
        /// when an invalid formula is encountered.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCellInvalidFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.SetContentsOfCell("a1", "=1++2");
            Assert.ThrowsException<FormulaFormatException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests the GetNamesOfAllNonemptyCells method in a simple case.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            sheet.SetContentsOfCell("b1", "=1/a1");
            sheet.SetContentsOfCell("c1", "=b1 + 1");
            sheet.SetContentsOfCell("d1", "=a1 - 2");

            IEnumerable<string> names = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(4, names.Count());
            Assert.IsTrue(names.Contains("a1"));
            Assert.IsTrue(names.Contains("b1"));
            Assert.IsTrue(names.Contains("c1"));
            Assert.IsTrue(names.Contains("d1"));
        }

        /// <summary>
        /// This tests the GetNamesOfAllNonemptyCells method in the case
        /// that there are no nonempty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCellsAllEmpty()
        {
            Spreadsheet sheet = new Spreadsheet();

            IEnumerable<string> names = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, names.Count());
        }

        /// <summary>
        /// This tests that the GetCellContent method throws an InvalidNameException
        /// when the given cell name is invalid.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.GetCellContents("1a");
            Assert.ThrowsException<InvalidNameException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests that the GetCellContentsMethod throws an InvalidNameException 
        /// when called on an empty string.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentEmptyName()
        {
            Spreadsheet sheet = new Spreadsheet();
            Action a = () => sheet.GetCellContents("");
            Assert.ThrowsException<InvalidNameException>(a, "failed to throw exception");
        }

        /// <summary>
        /// This tests the GetCellContent method in a simple case where
        /// a cell contains a double.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentDouble()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2.5");
            Assert.AreEqual(2.5, sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// This tests the GetCellContent method in a simple case where
        /// a cell contains a string.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "hi");
            Assert.AreEqual("hi", sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// This tests the GetCellContent method in a simple case where
        /// a cell contains a Formula.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula form = new Formula("2+2");
            sheet.SetContentsOfCell("a1", "=2+2");
            Assert.AreEqual(form, sheet.GetCellContents("a1"));
        }

        /// <summary>
        /// This tests the GetCellContent method returns an empty string
        /// when the cell with the given name is empty.
        /// </summary>
        [TestMethod]
        public void TestGetCellContentEmptyCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "2");
            Assert.AreEqual("", sheet.GetCellContents("b1"));
        }
    }
}