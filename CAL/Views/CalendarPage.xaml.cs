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
				try
				{
					await Task.Delay(1000);
					var calClient = DependencyService.Get<ICalClient>();
					var calendarsForUser = await calClient.GetCalendarsForUserAsync(new Guid(PreferencesManager.GetUserId()));

					var bc = (CalendarViewModel)BindingContext;
					bc.CurrentlySelectedCalendar = calendarsForUser.Calendars.SingleOrDefault(c => c.Id == new Guid(PreferencesManager.GetDefaultCalendarId()));

					foreach (var calendar in calendarsForUser.Calendars)
					{
						ToolbarItems.Add(new ToolbarItem
						{
							Order = ToolbarItemOrder.Secondary,
							Text = $"{calendar.Name} ({calendar.Color})",
							Command = bc.SelectCalendarCommand,
							CommandParameter = calendar,
						});
					}

					OnAppearing();
				}
				catch (Exception e)
				{
					var idk = 10;
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
