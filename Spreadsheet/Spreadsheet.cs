using SpreadsheetUtilities;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

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
    /// This file contains a Spreadsheet class that provides the internals needed for our eventual
    /// spreadsheet. It allows functionality such as setting and getting cell contents and values, and keeps 
    /// track of all dependencies.
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
        /// This is the validator for cell names.
        /// </summary>
        private Func<string, bool> IsValid;

        /// <summary>
        /// This is the normalizer for cell names.
        /// </summary>
        private Func<string, string> Normalize;

        /// <summary>
        /// This holds the version information of this spreadsheet.
        /// </summary>
        private string Version;

        /// <inheritdoc/>
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
        /// Constructs a spreadsheet by recording its variable validity test,
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
        /// Constructs a spreadsheet from the passed in XML file and records its variable validity test,
        /// its normalization method, and its version information. 
        /// </summary>
        /// <param name="filepath"> the path to the xml file to construct this spreadsheet from. </param>
        /// <param name="isValid"> defines what valid variables look like for the application </param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings </param>
        /// <param name="version"> defines the version of the spreadsheet (should it be saved) </param>
        /// <exception cref="SpreadsheetReadWriteException"> Thrown if the version of the saved spreadsheet does not match 
        /// the version parameter provided to the constructor, if any of the names contained in the saved spreadsheet are 
        /// invalid, if any invalid formulas or circular dependencies are encountered, if there are any problems opening, 
        /// reading, or closing the file, or if anything else goes wrong. </exception>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        {
            IsValid = isValid;
            Normalize = normalize;
            Changed = false;
            nonemptyCellMap = new Dictionary<string, Cell>();
            graph = new DependencyGraph();

            try
            {
                using (XmlReader reader = XmlReader.Create(filepath))
                {
                    string? name = null;
                    string? contents = null;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    string vers = reader["version"];
                                    if (vers != version)
                                    {
                                        throw new SpreadsheetReadWriteException("non-matching versions");
                                    }
                                    this.Version = vers;
                                    break;
                                case "cell":
                                    break;
                                case "name":
                                    reader.Read();
                                    name = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    contents = reader.Value;
                                    break;
                            }

                        }
                        if (name != null && contents != null)
                        {
                            this.SetContentsOfCell(name, contents);
                            name = null;
                            contents = null;
                        }
                    }
                }
            } 
            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("file error");
            } catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("invalid name");
            } catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("circular dependency");
            } catch (SpreadsheetReadWriteException)
            {
                throw new SpreadsheetReadWriteException("non-matching versions");
            } catch (FormulaFormatException)
            {
                throw new SpreadsheetReadWriteException("syntactically incorrect formula");
            } catch
            {
                throw new SpreadsheetReadWriteException("other issues");
            }
        }

        /// <summary>
        /// This is a helper method to determine if a given cell name is valid, meaning it 
        /// starts with one or more letters, ends with 1 or more digits, and is valid according
        /// to the Validator. 
        /// </summary>
        /// 
        /// <param name="name"> The name to determine the validity of. </param>
        /// 
        /// <exception cref="InvalidNameException"> Throws an InvalidNameExcpetion if the name found to be
        /// invalid by the prescribed requisites. </exception>
        private void DetermineIfNameIsInvalid(string name)
        {
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {
                throw new InvalidNameException();
            }

            if (!IsValid(name))
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
            name = Normalize(name);
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
                foreach (string variable in variables)
                {
                    GetCellsToRecalculate(variable);
                }
            }
            catch (CircularException)
            {
                // undo any dependency changes that caused cycle
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
            name = Normalize(name);
            DetermineIfNameIsInvalid(name);

            return graph.GetDependents(name);
        }

        /// <inheritdoc/>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = Normalize(name);
            DetermineIfNameIsInvalid(name);

            List<string> names;

            if (Double.TryParse(content, out double result))
            {
                names = new List<string>(SetCellContents(name, result));
            }

            else if (content.Length > 0 && content[0] == '=')
            {
                Formula form = new Formula(content.Substring(1), Normalize, IsValid);
                names = new List<string>(SetCellContents(name, form));
            }

            else
            {
                names = new List<string>(SetCellContents(name, content));
            }

            // must only recalculate the cells after the first cell in the list 
            // since that cell value has already been reset.
            RecalculateCellValues(names.GetRange(1, names.Count - 1));
            return names;
        }

        /// <summary>
        /// This is a helper method to be called in the SetContentsOfCell method
        /// that recalculates the values of all dependents of a cell that just
        /// changed.
        /// </summary>
        /// <param name="names"> The names of the cells to recalculate the values 
        /// of. </param>
        private void RecalculateCellValues(IList<string> names)
        {
            foreach (string name in names)
            {
                // all of these must hold Formulas since they're all dependents
                Cell cell = nonemptyCellMap[name];
                Formula form = (Formula)cell.getContent();
                object val = form.Evaluate(LookUp);
                cell.setValue(val);
            }
        }

        /// <inheritdoc/>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    string? vers = null;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    vers = reader["version"];
                                    if (vers == null)
                                    {
                                        throw new SpreadsheetReadWriteException("version not found");
                                    }
                                    return vers;
                                case "cell":
                                    break;
                                case "name":
                                    break;
                                case "contents":
                                    break;
                            }
                        }
                    }
                    if (vers == null)
                    {
                        throw new SpreadsheetReadWriteException("version not found");
                    }
                }
            } catch (IOException)
            {
                throw new SpreadsheetReadWriteException("file error");
            }
            catch (SpreadsheetReadWriteException)
            {
                throw new SpreadsheetReadWriteException("version not found");
            } catch
            {
                throw new SpreadsheetReadWriteException("other issues");
            }

            // this should never be encountered, but was included to ensure that all paths returned a value
            return "version not found";
        }

        /// <inheritdoc/>
        public override void Save(string filename)
        {
            try
            {
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
                            writer.WriteElementString("contents", (string)content);
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
            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("file error");
            } catch
            {
                throw new SpreadsheetReadWriteException("other errors");
            }
            Changed = false;
        }

        /// <inheritdoc/>
        public override object GetCellValue(string name)
        {
            name = Normalize(name);
            DetermineIfNameIsInvalid(name);

            if (!nonemptyCellMap.ContainsKey(name))
            {
                return "";
            }

            Cell cell = nonemptyCellMap[name];
            return cell.getValue();
        }

        /// <summary>
        /// This method will act as the lookup delegate to be passed into
        /// the Evaluate method for formulas. It either returns a double if 
        /// the passed in variable looks up to one, or throws an ArgumentException
        /// to be caught and dealt with by the Evaluate method.
        /// </summary>
        /// <param name="name"> The variable to look up. </param>
        /// <returns> What the variable looks up to be. </returns>
        /// <exception cref="ArgumentException"> Thrown if the variable does not look
        /// up to be a double. </exception>
        private double LookUp(string name)
        {
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

            /// <summary>
            /// This holds the value of a Cell (either a string, double, or FormulaError.
            /// </summary>
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
            /// <returns> Either a string, double, or Formula. </returns>
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

            /// <summary>
            /// This is a getter to get the value of this Cell object.
            /// </summary>
            /// <returns> Either a string, double, or FormulaError. </returns>
            public object getValue()
            {
                return value;
            }

            /// <summary>
            /// This is a setter to set the value of this Cell object to the input value.
            /// </summary>
            /// <param name="content"> The value to set the contents to. </param>
            public void setValue(object value)
            {
                this.value = value;
            }
        }
    }
}