//using Spreadsheet;
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
        /// <summary>
        /// Since every cell name already exists in a spreadsheet by default, this dictionary 
        /// will just hold a mapping between all non-empty cells (cells where their contents are 
        /// not the empty string) and their names. (where the keys are names and the values are 
        /// Cells). If a name is not in this map, the cell corresponding to that name is empty.
        /// </summary>
        private Dictionary<string, Cell> nonemptyCellMap;


        private DependencyGraph graph;

        public Spreadsheet()
        {
            nonemptyCellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        private static void DetermineIfNameIsInvalid(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"^[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerate 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return nonemptyCellMap.Keys;
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
            DetermineIfNameIsInvalid(name);

            if (nonemptyCellMap.ContainsKey(name))
            {
                Cell cell = nonemptyCellMap[name];
                return cell.getContent();
            }

            return "";
        }

        /// Every name already exists by default?
        /// 
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
            DetermineIfNameIsInvalid(name);

            // if the cell was previously empty, we don't need to alter dependencies
            if (!nonemptyCellMap.ContainsKey(name))
            {
                Cell newCell = new Cell(name, number);
                nonemptyCellMap.Add(name, newCell);
            }

            else
            {
                Cell cell = nonemptyCellMap[name];

                // if the cell used to contain a formula w variables, we must remove those dependencies
                graph.ReplaceDependees(name, new HashSet<string>());

                cell.setContent(number);
            }

            LinkedList<string> dependents = (LinkedList<string>)GetCellsToRecalculate(name);
            return new HashSet<string>(dependents);
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
            DetermineIfNameIsInvalid(name);

            if (text == null)
            {
                throw new ArgumentNullException();
            }

            // if the cell was previously empty, we don't need to alter dependencies
            if (!nonemptyCellMap.ContainsKey(name))
            {
                Cell newCell = new Cell(name, text);
                nonemptyCellMap.Add(name, newCell);
            }

            else
            {
                Cell cell = nonemptyCellMap[name];

                // if the cell used to contain a formula w variables, we must remove those dependencies
                graph.ReplaceDependees(name, new HashSet<string>());

                cell.setContent(text);
            }

            LinkedList<string> dependents = (LinkedList<string>)GetCellsToRecalculate(name);
            return new HashSet<string>(dependents);
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
            DetermineIfNameIsInvalid(name);

            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            // check for cycles
            ISet<string> variables = (ISet<string>)formula.GetVariables();
            GetCellsToRecalculate(variables);
            
            // now we know a CircularException was not thrown, so we can set the cell contents

            // if the cell was previously empty, 
            if (!nonemptyCellMap.ContainsKey(name))
            {
                Cell newCell = new Cell(name, formula);

                // since the cell was previously empty, it previously had no dependees, now must add the new ones
                graph.ReplaceDependees(name, variables);

                nonemptyCellMap.Add(name, newCell);
            }

            else
            {
                Cell cell = nonemptyCellMap[name];

                // if the cell used to contain a formula with variables, we must replace dependees,
                // otherwise we must only add dependees
                graph.ReplaceDependees(name, variables);

                cell.setContent(formula);
            }

            // finally return the set of dependents
            LinkedList<string> dependents = (LinkedList<string>)GetCellsToRecalculate(name);
            return new HashSet<string>(dependents);
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
        public override IEnumerable<string> GetDirectDependents(string name)
        {
            DetermineIfNameIsInvalid(name);

            return graph.GetDependents(name);
        }

        private class Cell
        {
            private string name;
            private Object content;
            //private Object value;

            public Cell(string name, Object content)
            {
                this.name = name;
                this.content = content;

                //if (content is string || content is double)
                //{
                //    this.value = content;
                //}

                //else if (content is Formula)
                //{
                //    // what do i do here?
                //    this.value = (Formula)content.Evaluate(null);
                //}

                //else
                //{
                //    // anything here?
                //}
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
