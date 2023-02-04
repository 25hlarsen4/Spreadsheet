using Spreadsheet;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SS
{
    /// <summary>
    /// <para>
    ///     An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    ///     spreadsheet consists of an infinite number of named cells.
    /// </para>
    /// <para>
    ///     A string is a valid cell name if and only if:
    /// </para>
    /// <list type="number">
    ///      <item> its first character is an underscore or a letter</item>
    ///      <item> its remaining characters (if any) are underscores and/or letters and/or digits</item>
    /// </list>   
    /// <para>
    ///     Note that this is the same as the definition of valid variable from the Formula class assignment.
    /// </para>
    /// 
    /// <para>
    ///     For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    ///     "25", "2x", and "&amp;" are not.  Cell names are case sensitive, so "x" and "X" are
    ///     different cell names.
    /// </para>
    /// 
    /// <para>
    ///     A spreadsheet contains a cell corresponding to every possible cell name.  (This
    ///     means that a spreadsheet contains an infinite number of cells.)  In addition to 
    ///     a name, each cell has a contents and a value.  The distinction is important.
    /// </para>
    /// 
    /// <para>
    ///     The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    ///     contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    ///     of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// </para>
    /// 
    /// <para>
    ///     In a new spreadsheet, the contents of every cell is the empty string. Note: 
    ///     this is by definition (it is IMPLIED, not stored).
    /// </para>
    /// 
    /// <para>
    ///     The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    ///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
    ///     in the grid.)
    /// </para>
    /// 
    /// <list type="number">
    ///   <item>If a cell's contents is a string, its value is that string.</item>
    /// 
    ///   <item>If a cell's contents is a double, its value is that double.</item>
    /// 
    ///   <item>
    ///      If a cell's contents is a Formula, its value is either a double or a FormulaError,
    ///      as reported by the Evaluate method of the Formula class.  The value of a Formula,
    ///      of course, can depend on the values of variables.  The value of a variable is the 
    ///      value of the spreadsheet cell it names (if that cell's value is a double) or 
    ///      is undefined (otherwise).
    ///   </item>
    /// 
    /// </list>
    /// 
    /// <para>
    ///     Spreadsheets are never allowed to contain a combination of Formulas that establish
    ///     a circular dependency.  A circular dependency exists when a cell depends on itself.
    ///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    ///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    ///     dependency.
    /// </para>
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> cellMap;
        private DependencyGraph graph;

        public Spreadsheet()
        {
            cellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerate 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return cellMap.Keys;
        }

        /// <summary>
        ///   Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is null or invalid
        /// </exception>
        /// 
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// 
        /// <returns>
        ///   The return value should be either a string, a double, or a Formula.
        ///   See the class header summary 
        /// </returns>
        public override object GetCellContents(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }

            // what if the name doesn't exist?????????????
            Cell cell = cellMap[name];
            return cell.getContent();
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///      The method returns a set consisting of name plus the names of all other cells whose value depends, 
        ///      directly or indirectly, on the named cell.
        ///   </para>
        /// 
        ///   <para>
        ///      For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///      set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }

            // what if the name doesn't exist?????????????
            Cell cell = cellMap[name];
            cell.setContent(number);

            // would just get direct dependents work?
            ISet<string> dependents = (ISet<string>)GetCellsToRecalculate(name);
            dependents.Add(name);
            return dependents;
        }

        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   The method returns a set consisting of name plus the names of all 
        ///   other cells whose value depends, directly or indirectly, on the 
        ///   named cell.
        /// 
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }

            // what if the name doesn't exist?????????????
            Cell cell = cellMap[name];
            cell.setContent(text);

            ISet<string> dependents = (ISet<string>)GetCellsToRecalculate(name);
            dependents.Add(name);
            return dependents;
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If formula parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name</param>
        /// <param name="formula"> The content of the cell</param>
        /// 
        /// <returns>
        ///   <para>
        ///     The method returns a Set consisting of name plus the names of all other 
        ///     cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///   <para> 
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// 
        /// </returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            if (name == null || !Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }

            // check if the cell name is in the formula??
            ISet<string> variables = (ISet<string>)formula.GetVariables();
            ISet<string> dependents = (ISet<string>)GetCellsToRecalculate(variables);
            dependents.Add(name);
            
            // now we know a CircularException was not thrown, so set the cell contents
            Cell cell = cellMap[name];
            cell.setContent(formula);

            // finally return the set of dependents
            return dependents;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. 
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If the name is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException ??????????????????????????
        /// </exception>
        /// 
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// 
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }

            if (!Regex.IsMatch(name, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }

            return graph.GetDependents(name);
        }

        private class Cell
        {
            private string name;
            private Object content;

            public Cell(string name, Object content)
            {
                this.name = name;
                this.content = content;
            }

            public Object getContent()
            {
                return content;
            }

            public void setContent(Object content)
            {
                this.content = content;
            }
        }
    }
}
