using System.Diagnostics;

namespace GUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            // initialize top labels

            for (int i = 64; i < 91; i++) {
                char ascii = (char)i;
                TopLabels.Add(
                new Border
                {
                    Stroke = Color.FromRgb(0, 0, 0),
                    StrokeThickness = 1,
                    HeightRequest = 20,
                    WidthRequest = 75,
                    HorizontalOptions = LayoutOptions.Center,
                    Content =
                        new Label
                        {
                            Text = ascii.ToString(),
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            HorizontalTextAlignment = TextAlignment.Center
                        }
                }
                );
            }



            for (int row = 0; row < 10; row++)
            {
                var horiz = new HorizontalStackLayout();

                // left column labels
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
                            Text = $"{ row + 1}",
                            BackgroundColor = Color.FromRgb(200, 200, 250),
                            VerticalTextAlignment = TextAlignment.Center
                        }
                    }
                );

                for (int i = 1; i < 26; i++) 
                {
                    var entry = new Entry
                    {
                        StyleId = ""
                    };

                    horiz.Add(entry);
                }

                Grid.Children.Add( horiz );
            }









            // initialize left labels

            //for (int i = 1; i < 21; i++)
            //{
            //    LeftLabels.Add(
            //    new Border
            //    {
            //        Stroke = Color.FromRgb(0, 0, 0),
            //        StrokeThickness = 1,
            //        HeightRequest = 20,
            //        WidthRequest = 75,
            //        HorizontalOptions = LayoutOptions.Center,
            //        Content =
            //            new Label
            //            {
            //                Text = i.ToString(),
            //                BackgroundColor = Color.FromRgb(200, 200, 250),
            //                HorizontalTextAlignment = TextAlignment.Center
            //            }
            //    }
            //    );
            //}

            //// initialize grid

            //for (int i = 64; i < 91; i++)
            //{
            //    VerticalStackLayout vert = new VerticalStackLayout();
            //    Nest.Add(vert);

            //    for (int j = 1; j < 21; j++)
            //    {
            //        vert.Add(
            //        new Border
            //        {
            //            Stroke = Color.FromRgb(0, 0, 0),
            //            StrokeThickness = 1,
            //            HeightRequest = 20,
            //            WidthRequest = 75,
            //            HorizontalOptions = LayoutOptions.Center,
            //            Content =
            //                new Label
            //                {
            //                    Text = i.ToString(),
            //                    BackgroundColor = Color.FromRgb(200, 200, 250),
            //                    HorizontalTextAlignment = TextAlignment.Center
            //                }
            //        }
            //        );
            //    }
            //}



            //for (int i = 1; i < 21; i++)
            //{
            //    Grid1.Add(
            //    new Border
            //    {
            //        Stroke = Color.FromRgb(0, 0, 0),
            //        StrokeThickness = 1,
            //        HeightRequest = 20,
            //        WidthRequest = 75,
            //        HorizontalOptions = LayoutOptions.Center,
            //        Content =
            //            new Label
            //            {
            //                Text = "",
            //                BackgroundColor = Color.FromRgb(200, 200, 250),
            //                HorizontalTextAlignment = TextAlignment.Center
            //            }
            //    }
            //    );
            //}

            //for (int i = 1; i < 21; i++)
            //{
            //    Grid2.Add(
            //    new Border
            //    {
            //        Stroke = Color.FromRgb(0, 0, 0),
            //        StrokeThickness = 1,
            //        HeightRequest = 20,
            //        WidthRequest = 75,
            //        HorizontalOptions = LayoutOptions.Center,
            //        Content =
            //            new Label
            //            {
            //                Text = "",
            //                BackgroundColor = Color.FromRgb(200, 200, 250),
            //                HorizontalTextAlignment = TextAlignment.Center
            //            }
            //    }
            //    );
            //}
        }

        private void FileMenuNew(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }

        private void FileMenuOpenAsync(object sender, EventArgs e)
        {
            Debug.WriteLine("here");
        }
    }
}