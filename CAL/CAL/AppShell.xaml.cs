using CAL.ViewModels;
using CAL.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CAL
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EventDetailPage), typeof(EventDetailPage));
            Routing.RegisterRoute(nameof(EditEventPage), typeof(EditEventPage));
        }

    }
}
