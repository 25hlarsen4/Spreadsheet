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
        //private readonly char[] COLHEADERS = "ABCDEFGHIJ".ToArray();
        //private readonly int ROWS = 10;

        //Spreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");

        //Dictionary<string, Entry> entries = new Dictionary<string, Entry>();

        private readonly char[] COLHEADERS;
        private readonly int ROWS;

        Spreadsheet ss;

        Dictionary<string, Entry> entries;

        Entry currEntry;

        public MainPage()
        {
            COLHEADERS = "ABCDEFGHIJ".ToArray();
            ROWS = 10;
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "six");
            entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();

            //if (entries.TryGetValue("A1", out Entry result))
            //{
            //    result.Focus();
            //}
            //entries.TryGetValue("A1", out Entry result);
            //Entry defaultFocused = entries.TryGetValue("A1", out string result);
            //defaultFocused.Focus();
        }

        public MainPage(string path)
        {
            COLHEADERS = "ABCDEFGHIJ".ToArray();
            ROWS = 10;
            ss = new Spreadsheet(path, s => true, s => s.ToUpper(), "six");
            entries = new Dictionary<string, Entry>();

            InitializeComponent();

            InitializeGrid();

            //if (entries.TryGetValue("A1", out Entry result))
            //{
            //    result.Focus();
            //}
            //entries.TryGetValue("A1", out Entry result);
            //Entry defaultFocused = entries.TryGetValue("A1", out string result);
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
                    //    //OnEntryFocused(entry, EventArgs.Empty);
                    //    entry.Focus();
                    //}

                    entries.Add(entry.StyleId, entry);

                    horiz.Add(entry);
                }

                Grid.Children.Add(horiz);
            }
        }

        private async void FileMenuNew(object sender, EventArgs e)
        {
            //    var popup = new Popup
            //    {
            //        Content = new VerticalStackLayout
            //        {
            //            Children =
            //{
            //    new Label
            //    {
            //        Text = "This is a very important message!"
            //    }
            //}
            //        }
            //    };



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

            // if they haven't provided a path to save to, display an error message
            //if (saveAs.Text == "")
            //{
            //    string path = await DisplayPromptAsync("", "Provide a file path to save to");

            //    //errorLabel.Text = "Cannot save without a file path, make sure you have provided one above.";
            //    //return;
            //}

            //string path = saveAs.Text + ".sprd";
            //try
            //{
            //    ss.Save(path);
            //} catch
            //{
            //    errorLabel.Text = "Issue encountered, make sure the provided file path is valid.";
            //}

        }

        private async void FileMenuHelp(object sender, EventArgs e)
        {
            await DisplayAlert("Help Menu", "Change selections by clicking. You can edit cell contents by clicking " +
                "a cell in the grid, typing the contents, and hitting enter, or by editing the contents label up top. " +
                "Furthermore, if you want to delete a cell with particularly long contents, instead of backspacing " +
                "everything and hitting enter, you can simply select the desired cell by clicking on it and then hit the" +
                "'clear entry' button provided.", "OK");
        }
        

        //private void OnSaveAsCompleted(object sender, EventArgs e)
        //{
        //    Entry ent = (Entry)sender;
        //    //saveAs.Text = ent.Text;
        //    // get rid of error message if there was one due to no file path being provided
        //    //errorLabel.Text = "";
        //}

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

        private void OnEntryTextChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

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

        private void OnEntryUnfocused(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string name = ent.StyleId;
            object val = ss.GetCellValue(name);
            ent.Text = val.ToString();
        }

        private void ClearEntry(object sender, EventArgs e)
        {
            currEntry.Text = "";
            OnEntryCompleted(currEntry, e);
        }
    }
}