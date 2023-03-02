using SpreadsheetUtilities;
using SS;

namespace GUI
{
    /// <summary>
    /// Author:     Hannah Larsen
    /// Partner:     Todd Oldham
    /// Date:        18-Feb-2023
    /// Course:      CS3500, University of Utah, School of Computing
    /// Copyright:   CS3500, Hannah Larsen, and Todd Oldham - This work may not be copied for use in academic coursework.
    /// 
    /// We, Hannah Larsen and Todd Oldham, certify that we wrote this code from scratch and did not copy it in part or whole 
    /// from another source.
    /// All references used in the completion of the assignment are cited in our README file.
    /// 
    /// File Contents:
    /// This class provides the backing code for the spreadsheet GUI xaml code. It allows for
    /// the creation of GUIs that represents Spreadsheet objects.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// This character array holds the column headers (A-Z)
        /// </summary>
        private readonly char[] COLHEADERS;

        /// <summary>
        /// This int represents the number of rows to be included in the spreadsheet.
        /// </summary>
        private readonly int ROWS;

        /// <summary>
        /// This is the underlying spreadsheet that the gui is to represent.
        /// </summary>
        Spreadsheet ss;

        /// <summary>
        /// This Dictionary maps cell names to their corresponding Entry.
        /// </summary>
        Dictionary<string, Entry> entries;

        /// <summary>
        /// This represents the most recently clicked entry, and will be the Entry
        /// to be cleared, should the clear entry button be clicked.
        /// </summary>
        Entry currEntry;

        /// <summary>
        /// Empty Constructor that initializes a gui representing an initially empty Spreadsheet.
        /// </summary>
        public MainPage()
        {
            COLHEADERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
            ROWS = 99;
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");
            entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();
        }

        /// <summary>
        /// Constructor that initializes a gui representing a Spreadsheet initialized from
        /// an input xml file.
        /// </summary>
        /// <param name="path"> The file path to the xml file to create the spreadsheet from. </param>
        public MainPage(string path)
        {
            COLHEADERS = "ABCDEFGHIJ".ToArray();
            ROWS = 10;
            ss = new Spreadsheet(path, s => true, s => s.ToUpper(), "six");
            entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();
        }

        /// <summary>
        /// Starter code provided by the professor
        /// used to setup all of the cells of the spreadsheet
        /// this method creates a label for all of the top labels
        /// and left labels and creates a new entry for each of the
        /// cells of the spreadsheet
        /// </summary>
        private void InitializeGrid()
        {
            // initialize upper-left corner
            TopLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 30,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = $"-",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
            );

            // initialize top labels
            foreach (var label in COLHEADERS)
            {
                TopLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 30,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = $"{label}",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }

            // initialize the rest of the grid cells
            for (int row = 0; row < ROWS; row++)
            {
                var horiz = new HorizontalStackLayout();

                // left column label
                horiz.Add(
                    new Border
                    {
                        Stroke = Color.FromRgb(0, 0, 0),
                        StrokeThickness = 1,
                        HeightRequest = 30,
                        WidthRequest = 75,
                        HorizontalOptions = LayoutOptions.Center,
                        Content =
                        new Label
                        {
                            Text = $"{row + 1}",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                    }
                );

                foreach (var label in COLHEADERS)
                {
                    var entry = new Entry
                    {
                        StyleId = $"{label}{row + 1}",
                        HeightRequest = 30,
                        WidthRequest = 75,
                    };
                    
                    // the cells should display their values by default
                    entry.Text = ss.GetCellValue(entry.StyleId).ToString();

                    entry.Completed += OnEntryCompleted;
                    entry.Focused += OnEntryFocused;
                    entry.Unfocused += OnEntryUnfocused;

                    entries.Add(entry.StyleId, entry);

                    horiz.Add(entry);
                }

                Grid.Children.Add(horiz);
            }
        }

        /// <summary>
        /// This method creates a new spreadsheet gui as a result of the new tab
        /// in the top file menu being clicked.
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private async void FileMenuNew(object sender, EventArgs e)
        {

            // if file has been changed without saving, issue warning
            if (ss.Changed)
            {
                bool answer = await DisplayAlert("Warning", "Possible data loss, do you want to save?", "Yes", "No");

                // if the user does want to save
                if (answer)
                {
                    FileMenuSave(sender, e);
                    // if the save was unsuccessful, leave the method
                    if (ss.Changed)
                    {
                        return;
                    }
                }
            }

            Application.Current.MainPage = new MainPage();
        }

        /// <summary>
        /// This method opens a gui representing an existing spreadsheet file.
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private async void FileMenuOpenAsync(object sender, EventArgs e)
        {
            // if file has been changed without saving, issue warning
            if (ss.Changed)
            {
                bool answer = await DisplayAlert("Warning", "Possible data loss, do you want to save?", "Yes", "No");

                // if the user does want to save
                if (answer)
                {
                    FileMenuSave(sender, e);
                    // if the save was unsuccessful, leave the method
                    if (ss.Changed)
                    {
                        return;
                    }
                }

            }

            string path = await DisplayPromptAsync("", "Provide a file to load from");
            path += ".sprd";

            Application.Current.MainPage = new MainPage(path);
        }

        /// <summary>
        /// This method is for saving spreadsheet guis, a path for the file to save to is 
        /// obtained and used to save.
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private async void FileMenuSave(object sender, EventArgs e)
        {
            string path = await DisplayPromptAsync("", "Provide a file path to save to");
            path += ".sprd";
            try
            {
                ss.Save(path);
            } catch
            {
                await DisplayAlert("Alert", "Invalid file path", "OK");
            }

        }

        /// <summary>
        /// This method is used to provide a help menu to the TAs to know how to use our
        /// spreadsheet gui.
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private async void FileMenuHelp(object sender, EventArgs e)
        {
            await DisplayAlert("Help Menu", "Change selections by clicking.\n" +
                "You can edit cell contents by clicking a cell in the grid, \n" +
                "typing the contents, and hitting enter, or by editing the contents label up top. \n" +
                "Furthermore, as a special feature, if you want to delete a cell with particularly long contents, \n" +
                "instead of backspacing everything and hitting enter, you can simply select the desired cell by clicking on it \n" +
                "and then hit the 'clear entry' button provided. Dependent cells will not be cleared in case you want the dependencies to remain. \n" +
                "To create a new spreadsheet, click file 'new' file. \n" +
                "To save a spreadsheet, click file 'save' file. \n" +
                "To open an existing spreadsheet, click file 'open' file."
                ,"OK");
        }

        /// <summary>
        /// When an entry in one of the cells is completed this method set that cell's contents, 
        /// updates each dependent cell's value, and displays said changes
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private async void OnEntryCompleted(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string cellName = ent.StyleId;
            string contents = ent.Text;

            try
            {
                IList<string> names = ss.SetContentsOfCell(cellName, contents);
                // update the dependent cells
                foreach (string name in names)
                {
                    if (entries.ContainsKey(name))
                    {
                        Entry cellEntry = entries[name];
                        cellEntry.Text = ss.GetCellValue(name).ToString();
                    }
                }

                OnEntryFocused(sender, e);

            } catch (FormulaFormatException)
            {
                await DisplayAlert("Alert", "Invalid formula.", "OK");
            }
            catch (CircularException)
            {
                await DisplayAlert("Alert", "Cycle encountered.", "OK");
            }

        }

        /// <summary>
        /// This method is used when one of the cells has been selected.
        /// When the cell is selected the contents should be displayed instead
        /// of the value
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private void OnEntryFocused(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            ent.Focus();

            // update the currently selected entry to the clear button knows what entry to clear if it is clicked
            currEntry = ent;

            selectedContents.StyleId = ent.StyleId;

            string name = ent.StyleId;
            object contents = ss.GetCellContents(name);

            string newText = "";
            // if the contents are a formula, put an = sign in front
            if (contents is Formula)
            {
                newText += "=";
            }
            newText += contents.ToString();

            ent.Text = newText;
            // display the contents
            selectedContents.Text = newText;

            object val = ss.GetCellValue(name);
            selectedCell.Text = "Name: " + name + "    Value: " + val.ToString();
        }

        /// <summary>
        /// When the cell is unfocused the value should be shown 
        /// instead of the contents
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private void OnEntryUnfocused(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string name = ent.StyleId;
            object val = ss.GetCellValue(name);
            ent.Text = val.ToString();
        }

        /// <summary>
        /// This method is used to clear an entry of the spreadsheet instead
        /// of having to manually empty the cell
        /// </summary>
        /// <param name="sender"> the sender </param>
        /// <param name="e"> the event args </param>
        private void ClearEntry(object sender, EventArgs e)
        {
            currEntry.Text = "";
            OnEntryCompleted(currEntry, e);
        }
    }
}