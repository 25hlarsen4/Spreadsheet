using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

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

        private Func<string, bool> IsValid;
        private Func<string, string> Normalize;
        private string Version;
        // { get => true; protected set => new string("a"); }

        public override bool Changed { get; protected set; }

        /// <summary>
        /// This creates an empty spreadsheet. An empty spreadsheet contains an infinite number of
        /// named cells, ie. a cell corresponding to every possible cell name.
        /// </summary>
        public Spreadsheet() :
            this(s => true, s => s, "default")
        {
            nonemptyCellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// This creates an empty spreadsheet. An empty spreadsheet contains an infinite number of
        /// named cells, ie. a cell corresponding to every possible cell name.
        /// </summary>
        //public Spreadsheet()
        //{
        //    nonemptyCellMap = new Dictionary<string, Cell>();
        //    graph = new DependencyGraph();
        //    Changed = false;
        //    IsValid = new Func<string, bool>(s => true);
        //    Normalize = new Func<string, string>(s => s);
        //    Version = "default";
        //}

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  
        /// </summary>
        /// 
        /// <remarks>
        ///   The variable validity test is used throughout to determine whether a string that consists of 
        ///   one or more letters followed by one or more digits is a valid cell name.  The variable
        ///   equality test should be used throughout to determine whether two variables are equal.
        /// </remarks>
        /// 
        /// <param name="isValid">   defines what valid variables look like for the application</param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="version">   defines the version of the spreadsheet (should it be saved)</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        {
            IsValid = isValid;
            Normalize = normalize;
            Version = version;
            nonemptyCellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// This is a helper method to determine if a given cell name is valid, meaning it is not null, 
        /// and it consists of an underscore or a letter followed by 0 or more underscores and/or letters
        /// and/or digits.
        /// </summary>
        /// 
        /// <param name="name"> The name to determine the validity of. </param>
        /// 
        /// <exception cref="InvalidNameException"> Throws an InvalidNameExcpetion if the name found to be
        /// invalid by the prescribed requisites. </exception>
        private void DetermineIfNameIsInvalid(string name)
        {
            if (!Regex.IsMatch(Normalize(name), @"^[a-zA-Z]+\d+$"))
            {
                throw new InvalidNameException();
            }

            if (!IsValid(Normalize(name)))
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

            // at this point we know the variable is valid, so normalize it
            name = Normalize(name);

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
        protected override IList<String> SetCellContents(string name, double number)
        {
            SetCellContentsHelper(name, number);
            Changed = true;
            Cell cell = nonemptyCellMap[name];
            cell.setValue(number);

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
        protected override IList<String> SetCellContents(string name, string text)
        {
            SetCellContentsHelper(name, text);
            Changed = true;
            Cell cell = nonemptyCellMap[name];
            cell.setValue(text);

            // if we set the cell contents to the empty string, it is now considered empty
            // so we must remove it from the nonemptyCellMap
            if (text == "")
            {
                nonemptyCellMap.Remove(name);
            }

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
        protected override IList<String> SetCellContents(string name, Formula formula)
        {
            // check for cycles
            ISet<string> variables = (ISet<string>)formula.GetVariables();
            foreach (string variable in variables)
            {
                graph.AddDependency(variable, name);
            }

            try
            {
                GetCellsToRecalculate(variables);
            } catch (CircularException)
            {
                foreach (string variable in variables)
                {
                    graph.RemoveDependency(variable, name);
                }
                throw new CircularException();
            }

            foreach (string variable in variables)
            {
                graph.RemoveDependency(variable, name);
            }

            // now we know a CircularException was not thrown, so we can set the cell contents
            SetCellContentsHelper(name, formula);
            Changed = true;
            Cell cell = nonemptyCellMap[name];
            cell.setValue(formula.Evaluate(LookUp));

            // update the dependees to be the variables in the new formula
            graph.ReplaceDependees(name, variables);

            return new List<string>(GetCellsToRecalculate(name));
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            DetermineIfNameIsInvalid(name);

            // at this point we know the variable is valid, so normalize it
            name = Normalize(name);

            return graph.GetDependents(name);
        }

        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            DetermineIfNameIsInvalid(name);

            // at this point we know the variable is valid, so normalize it
            name = Normalize(name);

            //IList<string> names;

            if (Double.TryParse(content, out double result))
            {
                //names = new List<string>(SetCellContents(name, result));
                //names = SetCellContents(name, result);
                List<string> names = new List<string>(SetCellContents(name, result));
                RecalculateCellValues(names.GetRange(1, names.Count - 1));
                return names;
            }

            // what if there's a space before the =???
            else if (content.Length > 0 && content[0] == '=')
            {
                Formula form = new Formula(content.Substring(1), Normalize, IsValid);
                //names = new List<string>(SetCellContents(name, form));
                //names = SetCellContents(name, form);
                List<string> names = new List<string>(SetCellContents(name, form));
                RecalculateCellValues(names.GetRange(1, names.Count - 1));
                return names;
            }

            else
            {
                //names = new List<string>(SetCellContents(name, content));
                //names = SetCellContents(name, content);
                List<string> names = new List<string>(SetCellContents(name, content));
                RecalculateCellValues(names.GetRange(1, names.Count - 1));
                return names;
            }

            //RecalculateCellValues(names);
            //return names;
        }

        private void RecalculateCellValues(IList<string> names)
        {
            // the first cell's value was already reset
            //names.RemoveAt(0);
            foreach (string name in names)
            {
                // do all of these have to hold Formulas since they're all dependents?
                // also since they hold Formulas, they're not empty, so i think we know they're in the map
                Cell cell = nonemptyCellMap[name];
                Formula form = (Formula)cell.getContent();
                object val = form.Evaluate(LookUp);
                cell.setValue(val);
            }
        }

        public override string GetSavedVersion(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            // REMEMBER TO DEAL WITH EXCEPTIONS

            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", this.Version);

                foreach (string name in this.GetNamesOfAllNonemptyCells())
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", name);

                    object content = GetCellContents(name);
                    if (content is string)
                    {
                        writer.WriteElementString("contents", name);
                    }
                    else if (content is double)
                    {
                        writer.WriteElementString("contents", content.ToString());
                    }
                    // otherwise it's a Formula
                    else
                    {
                        string formString = content.ToString();
                        string withEquals = "=" + formString;
                        writer.WriteElementString("contents", withEquals);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public override object GetCellValue(string name)
        {
            // check for validity

            if (!nonemptyCellMap.ContainsKey(name))
            {
                return "";
            }

            Cell cell = nonemptyCellMap[name];
            return cell.getValue();

            //if (cell.getContent() is string || cell.getContent() is double)
            //{
            //    return cell.getContent();
            //}

            //// otherwise it's a Formula
            //else
            //{
            //    Formula form = (Formula)cell.getContent();
            //    return form.Evaluate(LookUp);
            //}
        }

        private double LookUp(string name)
        {
            // is name already valid and normalized???

            if (!nonemptyCellMap.ContainsKey(name))
            {
                throw new ArgumentException();
            }

            Cell cell = nonemptyCellMap[name];

            if (cell.getContent() is string)
            {
                throw new ArgumentException();
            }

            else if (cell.getContent() is double)
            {
                return (double)cell.getContent();
            }

            // otherwise it's a Formula
            else
            {
                Formula form = (Formula)cell.getContent();
                object evaluation = form.Evaluate(LookUp);

                if (evaluation is FormulaError)
                {
                    throw new ArgumentException();
                }
                else
                {
                    return (double)evaluation;
                }
            }
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

            private object value = "";

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

            public object getValue()
            {
                return value;
            }

            public void setValue(object value)
            {
                this.value = value;
            }
        }
    }
}