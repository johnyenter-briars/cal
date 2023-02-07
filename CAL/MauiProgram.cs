using CAL.Client;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace CAL;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if ANDROID
			handlers.AddCompatibilityRenderer(typeof(ExtendedEntry),typeof(MauiAndroidKeyboard.Platforms.Android.Renderers.ExtendedEntryRenderer));
#endif


		//builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();

		return builder.Build();
	}
}
