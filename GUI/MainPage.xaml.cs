using System.Diagnostics;

namespace GUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
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