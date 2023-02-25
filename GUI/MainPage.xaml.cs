using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.Linq.Expressions;
//using static Java.Util.Jar.Attributes;
//using static System.Net.Mime.MediaTypeNames;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace GUI
{
    public partial class MainPage : ContentPage
    {

        private readonly char[] COLHEADERS;
        private readonly int ROWS;

        Spreadsheet ss;

        Dictionary<string, Entry> entries;

        Entry currEntry;

        /// <summary>
        /// Empty Constructor that initializes the spreadsheet
        /// </summary>
        public MainPage()
        {
            COLHEADERS = "ABCDEFGHIJ".ToArray();
            ROWS = 10;
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");
            entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();
        }

        /// <summary>
        /// Constructor that takes a path for the open file function
        /// of the spreadsheet
        /// </summary>
        /// <param name="path"></param>
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
                            VerticalTextAlignment = TextAlignment.Center
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

                    // don't forget to deal with string representation of ForumulaError
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
        /// This method creates a new spreadsheet from the new tab
        /// in the top file menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileMenuNew(object sender, EventArgs e)
        {

            // if file has been changed without saving, issue warning
            if (ss.Changed)
            {
                // display message to save
                bool answer = await DisplayAlert("Warning", "Possible data loss, do you want to save?", "Yes", "No");

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
        /// This method opens an existing spreadsheet file in the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileMenuOpenAsync(object sender, EventArgs e)
        {
            if (ss.Changed)
            {
                // display message to save
                bool answer = await DisplayAlert("Warning", "Possible data loss, do you want to save?", "Yes", "No");

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
        /// This method is for saving spreadsheets that have been changed
        /// a path for the file is obtained and used to save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FileMenuHelp(object sender, EventArgs e)
        {
            await DisplayAlert("Help Menu", "Change selections by clicking.\n" +
                " You can edit cell contents by clicking a cell in the grid, \n" +
                " typing the contents, and hitting enter, or by editing the contents label up top. \n" +
                "Furthermore, if you want to delete a cell with particularly long contents, \n" +
                " instead of backspacing everything and hitting enter, you can simply select the desired cell by clicking on it \n" +
                "and then hit the 'clear entry' button provided.\n" +
                "To create a new spreadsheet, click file 'new' file. \n" +
                "To save a spreadsheet, click file 'save' file. \n" +
                "To open an existing spreadsheet, click file 'open' file."
                , "OK");
        }

        /// <summary>
        /// When an entry in one of the cells is completed this method is called
        /// this calls set cell contents, and cell value and displays them in cells
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnEntryCompleted(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string cellName = ent.StyleId;
            string contents = ent.Text;

            try
            {
                IList<string> names = ss.SetContentsOfCell(cellName, contents);
                foreach (string name in names)
                {
                    // it should always contain name, right?
                    if (entries.ContainsKey(name))
                    {
                        Entry cellEntry = entries[name];
                        cellEntry.Text = ss.GetCellValue(name).ToString();
                    }
                }

                OnEntryFocused(sender, e);

            } catch (FormulaFormatException)
            {
                await DisplayAlert("Alert", "Invalid formula", "OK");
            }

        }

        /// <summary>
        /// This method is here because the Entries use the text changed to 
        /// display changed text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEntryTextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        /// <summary>
        /// This method is used when one of the cells has been selected.
        /// When the cell is selected the contents should be displayed instead
        /// of the value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (contents is Formula)
            {
                newText += "=";
            }
            newText += contents.ToString();

            ent.Text = newText;
            // should formula contents have = at the beginning?????
            selectedContents.Text = newText;

            object val = ss.GetCellValue(name);
            selectedCell.Text = "Name: " + name + "    Value: " + val.ToString();
        }

        /// <summary>
        /// When the cell is unfocused the value should be shown 
        /// instead of the contents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearEntry(object sender, EventArgs e)
        {
            currEntry.Text = "";
            OnEntryCompleted(currEntry, e);
        }
    }
}