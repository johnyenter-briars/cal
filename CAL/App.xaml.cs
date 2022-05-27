using CAL.Services;

namespace CAL;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        DependencyService.Register<EventsDataStore>();

        MainPage = new AppShell();
    }
}
