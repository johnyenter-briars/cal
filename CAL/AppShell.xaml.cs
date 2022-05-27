using CAL.Views;

namespace CAL;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EventDetailPage), typeof(EventDetailPage));
        Routing.RegisterRoute(nameof(EditEventPage), typeof(EditEventPage));
    }
}
