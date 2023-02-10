using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace SS
{
    /// <summary>
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
    /// This file contains a Spreadsheet class that provides the beginnings of the internals needed for our eventual
    /// spreadsheet. It allows functionality such as setting and getting cell contents and keeps track of all dependencies.
    /// See the inherited comments from the implemented AbstractSpreadsheet class for more details.
    /// </summary>
    /// 
    /// <inheritdoc/>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Since every cell name already exists in a spreadsheet by default, this dictionary 
        /// will just hold a mapping between all non-empty cells (cells where their contents are 
        /// not the empty string) and their names. (where the keys are names and the values are 
        /// Cells). If a name is not in this map, the cell corresponding to that name is empty.
        /// </summary>
        private Dictionary<string, Cell> nonemptyCellMap;

        /// <summary>
        /// This DependencyGraph will keep track of the dependencies between cells in a spreadsheet.
        /// </summary>
        private DependencyGraph graph;

        /// <summary>
        /// This creates an empty spreadsheet. An empty spreadsheet contains an infinite number of
        /// named cells, ie. a cell corresponding to every possible cell name.
        /// </summary>
        public Spreadsheet()
        {
            nonemptyCellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
        }

        /// <summary>
        /// This is a helper method to determine if a given cell name is valid, meaning it is not null, 
        /// and it consists of an underscore or a letter followed by zero or more underscores and/or letters
        /// and/or digits.
        /// </summary>
        /// 
        /// <param name="name"> The name to determine the validity of. </param>
        /// 
        /// <exception cref="InvalidNameException"> Throws an InvalidNameExcpetion if the name found to be
        /// invalid by the prescribed requisites. </exception>
        private static void DetermineIfNameIsInvalid(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"^[a-zA-Z_][a-zA-Z\d_]*$"))
            {
                throw new InvalidNameException();
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return nonemptyCellMap.Keys;
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// This is a helper method that is meant to be called in all SetCellContents methods. 
        /// It sets the contents of the cell with the given name to the specified string, double, 
        /// or Formula, making sure to keep up with any dependency changes that happen as a result.
        /// </summary>
        /// <param name="name"> The name of the cell to set the contents of. </param>
        /// <param name="contents"> The string, double, or Formula to set the cell contents to. </param>
        private void SetCellContentsHelper(string name, object contents)
        {
            // if the cell was previously empty, we don't need to alter dependees
            if (!nonemptyCellMap.ContainsKey(name))
            {
                Cell newCell = new Cell(name, contents);
                nonemptyCellMap.Add(name, newCell);
            }

            else
            {
                Cell cell = nonemptyCellMap[name];

                // since the cell was not empty, in case it used to contain a formula w variables,
                // we must remove those dependees
                graph.ReplaceDependees(name, new HashSet<string>());

                cell.setContent(contents);
            }
        }

        /// <inheritdoc/>
        public override ISet<string> SetCellContents(string name, double number)
        {
            DetermineIfNameIsInvalid(name);

            SetCellContentsHelper(name, number);

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
        public override ISet<string> SetCellContents(string name, string text)
        {
            DetermineIfNameIsInvalid(name);

            if (text == null)
            {
                throw new ArgumentNullException();
            }

            SetCellContentsHelper(name, text);

            // if we set the cell contents to the empty string, it is now considered empty
            // so we must remove it from the nonemptyCellMap
            if (text == "")
            {
                nonemptyCellMap.Remove(name);
            }

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
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
            SetCellContentsHelper(name, formula);

            // update the dependees to be the variables in the new formula
            graph.ReplaceDependees(name, variables);

            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            DetermineIfNameIsInvalid(name);

            return graph.GetDependents(name);
        }

        /// <summary>
        /// This private nested class allows the creation of Cell objects that have a name,
        /// contents, and a value. The contents of a cell can be a string, a double, or a Formula.
        /// If a cell's contents is a string, its value is that string.
        /// If a cell's contents is a double, its value is that double.
        /// If the contents is an empty string, the cell is considered empty.
        /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
        /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
        /// of course, can depend on the values of variables.  The value of a variable is the 
        /// value of the spreadsheet cell it names (if that cell's value is a double) or 
        /// is undefined (otherwise).
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// This holds the name of a Cell.
            /// </summary>
            private string name;

            /// <summary>
            /// This holds the contents of a Cell (either a string, double, or Formula)
            /// </summary>
            private Object content;

            /// <summary>
            /// This creates a Cell with the input name and contents.
            /// </summary>
            /// <param name="name"> The name for the Cell. </param>
            /// <param name="content"> The contents for the Cell. </param>
            public Cell(string name, Object content)
            {
                this.name = name;
                this.content = content;
            }

            /// <summary>
            /// This is a getter to get the contents of this Cell object.
            /// </summary>
            /// <returns></returns>
            public Object getContent()
            {
                return content;
            }

            /// <summary>
            /// This is a setter to set the contents of this Cell object to the input value.
            /// </summary>
            /// <param name="content"> The value to set the contents to. </param>
            public void setContent(Object content)
            {
                this.content = content;
            }
        }
    }
}
