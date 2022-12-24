using CAL.Client;

namespace CAL;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		DependencyService.RegisterSingleton(CalClientFactory.GetNewCalClient()
					//.UpdateSettings(PreferencesManager.GetHostname(),
					//				PreferencesManager.GetPort(),
					//				PreferencesManager.GetApiKey(),
					//				PreferencesManager.GetUserId())
					.UpdateSettings("192.168.0.6",
									8000,
									"test",
									"a188e597-29f9-4e2f-aa46-e3713d9939da")
					);

		MainPage = new AppShell();
	}
}
