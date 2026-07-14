using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SwiftScan.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwiftScan.ViewModels
{
    public partial class SetupViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private string _adminPin = string.Empty;

        [ObservableProperty]
        private string _confirmPin = string.Empty;

        [ObservableProperty]
        private string _selectedCurrency = "$";

        [ObservableProperty]
        private string _taxRate = "0";

        [ObservableProperty]
        private string _lowStockThreshold = "5";

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _hasError;

        public List<string> Currencies { get; } = new() { "$", "GH₵", "€", "£", "¥", "₹" };

        public SetupViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [RelayCommand]
        private async Task SaveAndCompleteSetupAsync()
        {
            ErrorMessage = string.Empty;
            HasError = false;

            if (string.IsNullOrWhiteSpace(AdminPin) || string.IsNullOrWhiteSpace(ConfirmPin))
            {
                ErrorMessage = "Please fill in all PIN fields.";
                HasError = true;
                return;
            }

            if (AdminPin != ConfirmPin)
            {
                ErrorMessage = "PINs do not match.";
                HasError = true;
                return;
            }

            if (AdminPin.Length < 4 || AdminPin.Length > 6 || !int.TryParse(AdminPin, out _))
            {
                ErrorMessage = "PIN must be a 4-6 digit number.";
                HasError = true;
                return;
            }

            if (!double.TryParse(TaxRate, out double taxVal) || taxVal < 0 || taxVal > 100)
            {
                ErrorMessage = "Please enter a valid tax rate (0-100%).";
                HasError = true;
                return;
            }

            if (!int.TryParse(LowStockThreshold, out int thresholdVal) || thresholdVal < 0)
            {
                ErrorMessage = "Please enter a valid low-stock threshold.";
                HasError = true;
                return;
            }

            // Save settings
            string hashedPin = _settingsService.HashPin(AdminPin);
            await _settingsService.SaveValueAsync("AdminPinHash", hashedPin);
            await _settingsService.SaveValueAsync("AdminPinLength", AdminPin.Length.ToString());
            await _settingsService.SaveValueAsync("CurrencySymbol", SelectedCurrency);
            await _settingsService.SaveValueAsync("TaxRate", TaxRate);
            await _settingsService.SaveValueAsync("LowStockThreshold", LowStockThreshold);
            await _settingsService.SaveValueAsync("IsSetupCompleted", "true");

            // Navigate to PinLockPage
            await Shell.Current.GoToAsync("///PinLockPage");
        }
    }
}
