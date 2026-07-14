using SwiftScan.Services;
using System.Threading.Tasks;

namespace SwiftScan
{
    public partial class AppShell : Shell
    {
        private readonly ISettingsService _settingsService;

        public AppShell(ISettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;

            // Route dynamically on application startup
            _ = CheckStartupRouteAsync();
        }

        private async Task CheckStartupRouteAsync()
        {
            // Allow MAUI context to fully initialize the visual tree and window before navigating
            await Task.Delay(100);

            string isSetupCompleted = await _settingsService.GetValueAsync("IsSetupCompleted", "false");

            if (isSetupCompleted == "true")
            {
                // Already set up, force authentication screen
                await GoToAsync("///PinLockPage");
            }
            else
            {
                // First run, force onboarding setup page
                await GoToAsync("///FirstTimeSetupPage");
            }
        }
    }
}
