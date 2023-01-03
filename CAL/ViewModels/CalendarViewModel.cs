﻿using CAL.Client;
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
using Event = CAL.Models.Event;
using System.Collections.Specialized;

namespace CAL.ViewModels
{
	internal class CalendarViewModel : BaseViewModel
	{
		public Command AddEventCommand => new(OnAddEvent);
		public Command AddSeriesCommand => new(OnAddSeries);
		public Command RefreshEventsCommand => new(Refresh);
		//public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
		public ICommand SelectCalendarCommand => new Command(async (item) => await SelectCalendar(item));
		public string SelectedCalendar { get; set; }
		//public EventCollection Events { get; } = new EventCollection();
		public ObservableRangeCollection<Event> Events { get; } = new ObservableRangeCollection<Event>()
		{
			new Event() { Title = "Bowling", Description = "Bowling with friends" },
			new Event() { Title = "Swimming", Description = "Swimming with friends" },
		};
		public Calendar<EventDay> EventCalendar { get; set; } = new Calendar<EventDay>()
		{
			SelectedDates = new ObservableRangeCollection<DateTime>(),
			SelectionAction = SelectionAction.Modify,
			SelectionType = SelectionType.Single
		};
		public static readonly Random Random = new Random();
		public List<Color> Colors { get; } = new List<Color>() { Microsoft.Maui.Graphics.Colors.Red, Microsoft.Maui.Graphics.Colors.Orange, Microsoft.Maui.Graphics.Colors.Yellow, Color.FromArgb("#00A000"), Microsoft.Maui.Graphics.Colors.Blue, Color.FromArgb("#8010E0") };
		public ObservableRangeCollection<Event> SelectedEvents { get; } = new ObservableRangeCollection<Event>();
		public ICommand NavigateCalendarCommand { get; set; }
		public ICommand ChangeDateSelectionCommand { get; set; }

		public DateTime _selectedDate;
		public Calendar CurrentlySelectedCalendar { get; set; }
		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set { SetProperty(ref _selectedDate, value); }
		}
		public CalendarViewModel(Calendar defaultCalendar)
		{
			CurrentlySelectedCalendar = defaultCalendar;
			Title = "Calendar";
			_selectedDate = DateTime.Now;
			NavigateCalendarCommand = new Command<int>(NavigateCalendar);
			ChangeDateSelectionCommand = new Command<DateTime>(ChangeDateSelection);

			foreach (Event Event in Events)
			{
				Event.DateTime = DateTime.Today.AddDays(1).AddSeconds(Random.Next(86400));
				Event.Color = Microsoft.Maui.Graphics.Colors.Red;
			}

			EventCalendar.SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;
			EventCalendar.DaysUpdated += EventCalendar_DaysUpdated;
			foreach (var Day in EventCalendar.Days)
			{
				Day.Events.ReplaceRange(Events.Where(x => x.DateTime.Date == Day.DateTime.Date));
			}


			Task.Run(async () => await ExecuteLoadEventsAsync());
		}
		public void NavigateCalendar(int Amount)
		{
			EventCalendar?.NavigateCalendar(Amount);
		}
		public void ChangeDateSelection(DateTime DateTime)
		{
			EventCalendar?.ChangeDateSelection(DateTime);
		}

		private async Task SelectCalendar(object calendar)
		{
			if (calendar is Calendar cal)
			{
				CurrentlySelectedCalendar = cal;
				var events = (await CalClientSingleton.GetEventsAsync()).Events.Where(e => e.CalendarId == CurrentlySelectedCalendar.Id).ToList();
				//LoadEventCollection(events, Events);
			}
		}
		//private void LoadEventCollection(IList<Event> events, EventCollection eventCollection)
		//{
		//	eventCollection.Clear();

		//	foreach (var e in events)
		//	{
		//		if (eventCollection.ContainsKey(e.StartTime))
		//		{
		//			//var listOfEvents = ((DayEventCollection<Event>)eventCollection[e.StartTime]);
		//			//listOfEvents.Add(e);
		//		}
		//		else
		//		{
		//			ColorTypeConverter converter = new ColorTypeConverter();
		//			Color color = (Color)(converter.ConvertFromInvariantString(CurrentlySelectedCalendar.Color));

		//			//var dayEventCollection = new DayEventCollection<Event>(color, color) { e };
		//			eventCollection.Add(e.StartTime, dayEventCollection);
		//		}
		//	}
		//}
		private async Task ExecuteLoadEventsAsync()
		{
			IsBusy = true;

			try
			{
				var events = (await CalClientSingleton.GetEventsAsync()).Events.Where(e => e.CalendarId == CurrentlySelectedCalendar.Id).ToList();
				//LoadEventCollection(events, Events);
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
			await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
		}
		private async void OnAddSeries()
		{
			var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
			var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddDays(1)).ToUnixTimeSeconds();
			await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
		}
		public async void Refresh()
		{
			await ExecuteLoadEventsAsync();
		}
		private void SelectedDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			SelectedEvents.ReplaceRange(Events.Where(x => EventCalendar.SelectedDates.Any(y => x.DateTime.Date == y.Date)).OrderByDescending(x => x.DateTime));
		}
		private void EventCalendar_DaysUpdated(object sender, EventArgs e)
		{
			foreach (var Day in EventCalendar.Days)
			{
				Day.Events.ReplaceRange(Events.Where(x => x.DateTime.Date == Day.DateTime.Date));
			}
		}

		//private async Task ExecuteEventSelectedCommand(object item)
		//{
		//	if (item is Event e)
		//	{
		//		if (e.SeriesId != null)
		//		{
		//			var series = (await DependencyService.Get<ICalClient>().GetSeriesAsync((Guid)e.SeriesId)).Series;
		//			var startUnixTimeSeconds = ((DateTimeOffset)series.StartsOn.Add(series.EventStartTime).ToUniversalTime()).ToUnixTimeSeconds();
		//			var endUnixTimeSeconds = ((DateTimeOffset)series.EndsOn.Add(series.EventEndTime).ToUniversalTime()).ToUnixTimeSeconds();
		//			await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}&{nameof(EditSeriesViewModel.EntityType)}={series.EntityType}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
		//		}
		//		else
		//		{
		//			var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
		//			var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
		//			await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}&{nameof(EditEventViewModel.EntityType)}={e.EntityType}");
		//		}
		//	}
		//}
	}
}
