using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SwiftScan.Services;
using System.Threading.Tasks;

namespace SwiftScan.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private string _currencySymbol = "$";

        [ObservableProperty]
        private string _totalRevenueDisplay = "$0.00";

        public MainPageViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task LoadDashboardDataAsync()
        {
            string symbol = await _settingsService.GetValueAsync("CurrencySymbol", "$");
            CurrencySymbol = symbol;
            TotalRevenueDisplay = $"{symbol}0.00";
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await Shell.Current.GoToAsync("///PinLockPage");
        }
    }
}
