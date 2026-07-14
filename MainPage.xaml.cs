using System;

namespace SwiftScan
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLockClicked(object sender, EventArgs e)
        {
            // Redirect back to PIN authentication page
            await Shell.Current.GoToAsync("///PinLockPage");
        }
    }
}
