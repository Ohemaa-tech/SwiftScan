using SwiftScan.ViewModels;

namespace SwiftScan.Views
{
    public partial class PinLockPage : ContentPage
    {
        private readonly PinLockViewModel _viewModel;

        public PinLockPage(PinLockViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadSettingsAsync();
        }
    }
}
