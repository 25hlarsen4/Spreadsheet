using SS;
using System.Diagnostics;
//using static AndroidX.Concurrent.Futures.CallbackToFutureAdapter;

namespace GUI
{
    public partial class MainPage : ContentPage
    {
        private readonly char[] COLHEADERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
        private readonly int ROWS = 10;

        public MainPage()
        {
            InitializeComponent();

            InitializeGrid();

            Spreadsheet ss = new Spreadsheet();
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

                for (int i = 1; i < 26; i++)
                {
                    var entry = new Entry
                    {
                        StyleId = "",
                        HeightRequest = 30,
                        WidthRequest = 75,
                    };

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
            Debug.WriteLine("OnEntryCompleted");
        }
    }
}