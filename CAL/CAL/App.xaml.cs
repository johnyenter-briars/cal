using CAL.Client;
using CAL.Managers;
using CAL.Services;
using CAL.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAL
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<EventsDataStore>();
            DependencyService.RegisterSingleton(CalClientFactory.GetNewCalClient().
                    UpdateSettings(PreferencesManager.GetHostname(),
                                    PreferencesManager.GetPort(),
                                    PreferencesManager.GetApiKey(),
                                    PreferencesManager.GetUserId())
                    );

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
