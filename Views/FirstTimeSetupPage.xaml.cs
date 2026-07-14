using SwiftScan.ViewModels;

namespace SwiftScan.Views
{
    public partial class FirstTimeSetupPage : ContentPage
    {
        public FirstTimeSetupPage(SetupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
