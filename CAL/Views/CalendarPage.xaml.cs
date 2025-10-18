using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Managers;
using CAL.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                await Task.Delay(1000);
                var calClient = DependencyService.Get<ICalClient>();
                var calendarsForUser = await calClient.GetCalendarsForUserAsync(new Guid(PreferencesManager.GetUserId()));

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


                OnAppearing();
            });
        }
        protected override void OnAppearing()
        {
            var bc = (CalendarViewModel)BindingContext;

            bc?.Refresh();
        }
    }
}
