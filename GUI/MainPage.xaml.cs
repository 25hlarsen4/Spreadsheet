using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
//using static Java.Util.Jar.Attributes;
//using static System.Net.Mime.MediaTypeNames;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace GUI
{
    public partial class MainPage : ContentPage
    {
        private readonly char[] COLHEADERS = "ABCDEFGHIJ".ToArray();
        private readonly int ROWS = 10;

        Spreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");

        Dictionary<string, Entry> entries = new Dictionary<string, Entry>();

        //private readonly char[] COLHEADERS;
        //private readonly int ROWS;

        //Spreadsheet ss;

        //Dictionary<string, Entry> entries;

        public MainPage()
        {
            //COLHEADERS = "ABCDEFGHIJ".ToArray();
            //ROWS = 10;
            //ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");
            //entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();

            //Entry defaultFocused = entries["A1"];
            //defaultFocused.Focus();
        }

        private void InitializeTopBar()
        {

        }

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

                    //if (entry.StyleId == "A1")
                    //{
                    //    OnEntryFocused(entry, EventArgs.Empty);
                    //}

                    entries.Add(entry.StyleId, entry);

                    horiz.Add(entry);
                }

                Grid.Children.Add(horiz);
            }
        }

        private void FileMenuNew(object sender, EventArgs e)
        {
            if (ss.Changed == true)
            {
                // display message to save
                return;
            }
            Application.Current.MainPage = new MainPage();
        }

        private void FileMenuOpenAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        private void FileMenuSave(object sender, EventArgs e)
        {
            // if they haven't provided a path to save to, display an error message
            if (saveAs.Text == "")
            {
                errorLabel.Text = "Cannot save without a file path, make sure you have provided one above.";
                return;
            }

            string path = saveAs.Text + ".sprd";
            try
            {
                ss.Save(path);
            } catch
            {
                errorLabel.Text = "Issue encountered, make sure the provided file path is valid.";
            }
            
        }

        private void OnSaveAsCompleted(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            saveAs.Text = ent.Text;
            // get rid of error message if there was one due to no file path being provided
            errorLabel.Text = "";
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string cellName = ent.StyleId;
            string contents = ent.Text;

            try
            {
                errorLabel.Text = "";
                IList<string> names = ss.SetContentsOfCell(cellName, contents);
                foreach (string name in names)
                {
                    // it should always contain name, right?
                    if (entries.ContainsKey(name))
                    {
                        Entry cellEntry = entries[name];
                        cellEntry.Text = ss.GetCellValue(cellName).ToString();
                    }
                }
            } catch (FormulaFormatException)
            {
                errorLabel.Text = "Invalid Formula";
            }

        }

        private void OnEntryTextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        private void OnEntryFocused(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            ent.Focus();

            selectedContents.StyleId = ent.StyleId;

            string name = ent.StyleId;
            object contents = ss.GetCellContents(name);
            ent.Text= contents.ToString();
            // should formula contents have = at the beginning?????
            selectedContents.Text = contents.ToString();

            object val = ss.GetCellValue(name);
            selectedCell.Text = "Name: " + name + " Value: " + val.ToString();
        }

        private void OnEntryUnfocused(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string name = ent.StyleId;
            object val = ss.GetCellValue(name);
            ent.Text = val.ToString();
        }
    }
}