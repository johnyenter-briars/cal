using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Models;
using CAL.Views;
using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using XCalendar.Core.Interfaces;
using XCalendar.Core.Models;
using System.Runtime.CompilerServices;
using XCalendar.Core.Enums;
using System.Collections.Specialized;
using Microsoft.Maui.Graphics;

namespace CAL.ViewModels
{
	internal class CalendarViewModel : BaseViewModel
	{
		public Command AddEventCommand => new(OnAddEvent);
		public Command AddSeriesCommand => new(OnAddSeries);
		public Command RefreshEventsCommand => new(Refresh);
		//public Command EventSelectedCommand => new(ExecuteEventSelectedCommand);

		private CalendarEvent selectedEvent;
		public CalendarEvent SelectedEvent
		{
			get { return selectedEvent; }
			set
			{
				selectedEvent = value;
				ExecuteEventSelectedCommand();
			}
		}
		public ObservableRangeCollection<CalendarEvent> EventsBuffer { get; } = new ObservableRangeCollection<CalendarEvent>()
		{
		};
		public Calendar<EventDay> EventCalendar { get; set; } = new Calendar<EventDay>()
		{
			SelectedDates = new ObservableRangeCollection<DateTime>(),
			SelectionType = SelectionType.Single,
		};
		public ObservableRangeCollection<CalendarEvent> SelectedEvents { get; } = new ObservableRangeCollection<CalendarEvent>();
		public ICommand NavigateCalendarCommand { get; set; }
		public ICommand ChangeDateSelectionCommand { get; set; }
		public DateTime _selectedDate;
		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set { SetProperty(ref _selectedDate, value); }
		}
		public CalendarViewModel()
		{
			Title = "Calendar";
			_selectedDate = DateTime.Now;
			NavigateCalendarCommand = new Command<int>(NavigateCalendar);
			ChangeDateSelectionCommand = new Command<DateTime>(ChangeDateSelection);


			EventCalendar.SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;

			EventCalendar.DaysUpdated += EventCalendar_DaysUpdated;

			foreach (var Day in EventCalendar.Days)
			{
				Day.Events.ReplaceRange(EventsBuffer.Where(x => x.StartTime.Date == Day.DateTime.Date));
			}

			//Task.Run(async () => await ExecuteLoadEventsAsync());
		}
		public void NavigateCalendar(int Amount)
		{
			EventCalendar?.NavigateCalendar(Amount);
		}
		public void ChangeDateSelection(DateTime DateTime)
		{
			EventCalendar?.ChangeDateSelection(DateTime);
		}
		private void LoadEventCollection(IList<Event> events)
		{
			EventsBuffer.AddRange(events.Select(d => new CalendarEvent
			{
				Id = d.Id,
				StartTime = d.StartTime,
				EndTime = d.EndTime,
				Name = d.Name,
				Description = d.Description,
				CalUserId = d.CalUserId,
				SeriesId = d.SeriesId,
				CalendarId = d.CalendarId,
				Color = Color.Parse(d.Color),
				SeriesName = d.SeriesName,
			}));

			foreach (var Day in EventCalendar.Days)
			{
				var filtered = EventsBuffer.Where(x => x.StartTime.Date == Day.DateTime.Date);
				var idk = filtered;
				Day.Events.ReplaceRange(filtered);
			}
		}

		private async Task ExecuteLoadEventsAsync()
		{
			IsBusy = true;

			try
			{
				var today = DateTime.Now;
				var events = await CalClientSingleton.GetEventsAsync(today.Year, today.Month);
				LoadEventCollection(events.Events);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
		private async void OnAddEvent()
		{
			var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
			var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddHours(1)).ToUnixTimeSeconds();
			//await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
		}
		private async void OnAddSeries()
		{
			var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
			var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddDays(1)).ToUnixTimeSeconds();
			//await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
		}
		public async void Refresh()
		{
			await ExecuteLoadEventsAsync();
		}
		private void SelectedDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SelectedEvents.ReplaceRange(EventsBuffer.Where(x => EventCalendar.SelectedDates.Any(y => x.StartTime.Date == y.Date)).OrderByDescending(x => x.StartTime));
		}
		private void EventCalendar_DaysUpdated(object sender, EventArgs e)
		{
			foreach (var Day in EventCalendar.Days)
			{
				Day.Events.ReplaceRange(EventsBuffer.Where(x => x.StartTime.Date == Day.DateTime.Date));
			}
		}

		private async void ExecuteEventSelectedCommand()
		{
			var e = SelectedEvent;

			if (e.SeriesId != null)
			{
				var series = (await DependencyService.Get<ICalClient>().GetSeriesAsync((Guid)e.SeriesId)).Series;
				var startUnixTimeSeconds = ((DateTimeOffset)series.StartsOn.Add(series.EventStartTime).ToUniversalTime()).ToUnixTimeSeconds();
				var endUnixTimeSeconds = ((DateTimeOffset)series.EndsOn.Add(series.EventEndTime).ToUniversalTime()).ToUnixTimeSeconds();
				//await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}&{nameof(EditSeriesViewModel.EntityType)}={series.EntityType}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
				await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}&{nameof(EditSeriesViewModel.EntityType)}={series.EntityType}");
			}
			else
			{
				var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
				var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
				await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}&{nameof(EditEventViewModel.EntityType)}={e.EntityType}");
			}
		}
	}
}
