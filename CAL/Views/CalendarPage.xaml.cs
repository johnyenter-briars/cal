using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Managers;
using CAL.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CAL.Views
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            var viewModel = new CalendarViewModel(null);

            BindingContext = viewModel;

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000);
                    var calClient = DependencyService.Get<ICalClient>();
                    var calendarsForUser = await calClient.GetCalendarsForUserAsync(new Guid(PreferencesManager.GetUserId()));

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        var bc = (CalendarViewModel)BindingContext;
                        bc.CurrentlySelectedCalendar = calendarsForUser.Calendars.SingleOrDefault(c => c.Id == new Guid(PreferencesManager.GetDefaultCalendarId()));

                        ToolbarItems.Add(new ToolbarItem
                        {
                            Text = "Calendars",
                            IconImageSource = "icons8_calendar_50.png",
                            Command = new Command(async () =>
                            {
                                var selected = await DisplayActionSheet("Select Calendar", "Cancel", null,
                                    calendarsForUser.Calendars.Select(c => $"{c.Name} ({c.Color})").ToArray());

                                if (selected != null && selected != "Cancel")
                                {
                                    var cal = calendarsForUser.Calendars.First(c => $"{c.Name} ({c.Color})" == selected);
                                    bc.SelectCalendarCommand.Execute(cal);
                                }
                            })
                        });
                    });


                    await MainThread.InvokeOnMainThreadAsync(OnAppearing);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);

                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await DisplayAlert(
                            "Network Error",
                            $"Unable to reach the calendar server. Please check your connection and try again.\n\nDetails: {ex.Message}",
                            "OK");
                    });
                }
            });
        }
        protected override void OnAppearing()
        {
            var bc = (CalendarViewModel)BindingContext;

            bc?.Refresh();
        }
    }
}
