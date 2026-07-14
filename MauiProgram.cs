using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SwiftScan.Services;
using SwiftScan.ViewModels;
using SwiftScan.Views;

namespace SwiftScan;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register Services
		builder.Services.AddSingleton<DatabaseContext>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();

		// Register ViewModels
		builder.Services.AddTransient<SetupViewModel>();
		builder.Services.AddTransient<PinLockViewModel>();

		// Register Pages
		builder.Services.AddTransient<FirstTimeSetupPage>();
		builder.Services.AddTransient<PinLockPage>();
		builder.Services.AddTransient<MainPage>();

		// Register AppShell
		builder.Services.AddSingleton<AppShell>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
