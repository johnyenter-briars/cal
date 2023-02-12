using CAL.Client;
using CAL.Managers;

namespace CAL;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		DependencyService.RegisterSingleton(CalClientFactory.GetNewCalClient()
					.UpdateSettings(PreferencesManager.GetHostname(),
									PreferencesManager.GetPort(),
									PreferencesManager.GetApiKey(),
									PreferencesManager.GetUserId())
					);

		MainPage = new AppShell();
	}
}
