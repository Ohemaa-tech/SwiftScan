using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SwiftScan.Services;
using System.Threading.Tasks;

namespace SwiftScan.ViewModels
{
    public partial class PinLockViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private string _storedPinHash = string.Empty;
        private int _expectedPinLength = 4;

        [ObservableProperty]
        private string _pinEntry = string.Empty;

        [ObservableProperty]
        private string _pinMask = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _hasError;

        public PinLockViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task LoadSettingsAsync()
        {
            _storedPinHash = await _settingsService.GetValueAsync("AdminPinHash");
            string lengthStr = await _settingsService.GetValueAsync("AdminPinLength", "4");
            if (int.TryParse(lengthStr, out int length))
            {
                _expectedPinLength = length;
            }
            else
            {
                _expectedPinLength = 4;
            }
            ClearPin();
        }

        [RelayCommand]
        private void PressKey(string digit)
        {
            if (PinEntry.Length >= _expectedPinLength)
                return;

            PinEntry += digit;
            UpdatePinMask();
            ErrorMessage = string.Empty;
            HasError = false;

            if (PinEntry.Length == _expectedPinLength)
            {
                // Run validation in background to avoid blocking keypress UI responsiveness
                _ = ValidatePinAsync();
            }
        }

        [RelayCommand]
        private void Delete()
        {
            if (PinEntry.Length > 0)
            {
                PinEntry = PinEntry[..^1];
                UpdatePinMask();
            }
        }

        [RelayCommand]
        private void Clear()
        {
            ClearPin();
        }

        private void ClearPin()
        {
            PinEntry = string.Empty;
            UpdatePinMask();
        }

        private void UpdatePinMask()
        {
            var mask = new System.Text.StringBuilder();
            for (int i = 0; i < _expectedPinLength; i++)
            {
                if (i < PinEntry.Length)
                {
                    mask.Append("● ");
                }
                else
                {
                    mask.Append("○ ");
                }
            }
            PinMask = mask.ToString().TrimEnd();
        }

        private async Task ValidatePinAsync()
        {
            // Give user a brief visual feedback before evaluating and clearing
            await Task.Delay(150);

            string enteredHash = _settingsService.HashPin(PinEntry);
            if (enteredHash == _storedPinHash)
            {
                ClearPin();
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                ErrorMessage = "Incorrect PIN";
                HasError = true;
                ClearPin();
            }
        }
    }
}
