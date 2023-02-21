﻿using SS;
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

        Spreadsheet ss = new Spreadsheet();

        public MainPage()
        {
            InitializeComponent();

            InitializeGrid();
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

                    horiz.Add(entry);
                }

                Grid.Children.Add(horiz);
            }
        }

        private void FileMenuNew(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        private void FileMenuOpenAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            Entry ent = (Entry)sender;
            string cellName = ent.StyleId;
            string contents = ent.Text;
            ss.SetContentsOfCell(cellName, contents);
        }
    }
}