using CAL.Views;

namespace CAL;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EditSeriesPage), typeof(EditSeriesPage));
        Routing.RegisterRoute(nameof(EditEventPage), typeof(EditEventPage));
        Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
    }
}
