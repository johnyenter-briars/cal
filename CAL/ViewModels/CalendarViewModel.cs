using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Models;
using CAL.Views;
using System.Diagnostics;
using System.Windows.Input;
using XCalendar.Core.Models;
using XCalendar.Core.Enums;
using System.Collections.Specialized;
using CAL.Managers;
using XCalendar.Core.Collections;
using XCalendar.Core.Extensions;

namespace CAL.ViewModels
{
    [QueryProperty(nameof(CurrentlySelectedCalendarId), nameof(CurrentlySelectedCalendarId))]
    [QueryProperty(nameof(NavigatingToEvent), nameof(NavigatingToEvent))]
    [QueryProperty(nameof(OpenDateStartTimeUnixSeconds), nameof(OpenDateStartTimeUnixSeconds))]
    internal class CalendarViewModel : BaseViewModel
    {
        private string currentlySelectedCalendarId;
        public string CurrentlySelectedCalendarId
        {
            get => currentlySelectedCalendarId;
            set
            {
                currentlySelectedCalendarId = value;
            }
        }
        private long openDateStartTimeUnixSeconds;
        public long OpenDateStartTimeUnixSeconds
        {
            get => openDateStartTimeUnixSeconds;
            set
            {
                openDateStartTimeUnixSeconds = value;
            }
        }
        private static List<string> SupportedColors = new List<string> {
                    "Red",
                    "Blue",
                    "Green",
                    "Orange",
                    "Yellow",
                    "Purple",
                    "Pink",
                };
        public Command AddEventCommand => new(OnAddEvent);
        public Command AddSeriesCommand => new(OnAddSeries);
        public Command RefreshEventsCommand => new(Refresh);
        public ICommand SelectCalendarCommand => new Command<CAL.Client.Models.Cal.Calendar>(async (item) => await SelectCalendar(item));

        private bool navigatingToEvent;
        public bool NavigatingToEvent
        {
            get { return navigatingToEvent; }
            set
            {
                navigatingToEvent = value;
            }
        }
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
        /// <summary>
        /// Represents the events selected in the current bucket (usually all events in the current month given the currently selected calendar)
        /// </summary>
        public ObservableRangeCollection<XCalendar.Core.Models.Event> EventsBuffer { get; } = [];
        public Calendar<EventDay> EventCalendar { get; set; } = new Calendar<EventDay>()
        {
            SelectedDates = new ObservableRangeCollection<DateTime>(),
            SelectionType = SelectionType.Single,
            SelectionAction = SelectionAction.Replace,
        };
        /// <summary>
        /// Events in the dropdown when a day(s) is selected
        /// </summary>
        public ObservableRangeCollection<XCalendar.Core.Models.Event> SelectedEvents { get; } = [];
        public CAL.Client.Models.Cal.Calendar CurrentlySelectedCalendar { get; set; }
        public ICommand NavigateCalendarCommand { get; set; }
        public ICommand ChangeDateSelectionCommand { get; set; }
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                var oldSelectedDate = _selectedDate;
                SetProperty(ref _selectedDate, value);
                var monthsDiff = ((_selectedDate.Year - oldSelectedDate.Year) * 12) + _selectedDate.Month - oldSelectedDate.Month;

                NavigateCalendar(monthsDiff);
            }
        }
        public CalendarViewModel(CAL.Client.Models.Cal.Calendar defaultCalendar)
        {
            Title = "Calendar";
            CurrentlySelectedCalendar = defaultCalendar;
            _selectedDate = DateTime.Now;

            NavigateCalendarCommand = new Command<int>(NavigateCalendar);
            ChangeDateSelectionCommand = new Command<DateTime>(ChangeDateSelection);

            EventCalendar.SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;

            EventCalendar.DaysUpdated += EventCalendar_DaysUpdated;
        }
        public async void NavigateCalendar(int amount)
        {
            if (EventCalendar.NavigatedDate.TryAddMonths(amount, out DateTime targetDate))
            {
                EventCalendar.Navigate(targetDate - EventCalendar.NavigatedDate);
            }
            else
            {
                EventCalendar.Navigate(amount > 0 ? TimeSpan.MaxValue : TimeSpan.MinValue);
            }

            var month = EventCalendar.NavigatedDate.Month;
            var year = EventCalendar.NavigatedDate.Year;
            await LoadEventCollectionAsync(year, month);
        }
        public void ChangeDateSelection(DateTime DateTime)
        {
            EventCalendar?.ChangeDateSelection(DateTime);
        }
        private async Task LoadEventCollectionAsync(int year, int month)
        {
            var eventsOfMonth = (await CalClientSingleton.GetEventsAsync(year, month));
            var events = eventsOfMonth.Events.Where(e => e.CalendarId == CurrentlySelectedCalendar?.Id).ToList();

            EventsBuffer.Clear();
            EventsBuffer.AddRange(events.Select(d => new XCalendar.Core.Models.Event
            {
                //Id = d.Id,
                StartDate = d.StartTime,
                EndDate = d.EndTime,
                Title = d.Name,
                Description = d.Description,
            }));

            foreach (var Day in EventCalendar.Days)
            {
                var filtered = EventsBuffer.Where(x => x.StartDate.Date == Day.DateTime.Date);
                Day.Events.ReplaceRange(filtered);
            }
        }

        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                var today = DateTime.Now;
                var month = EventCalendar.NavigatedDate.Month;
                var year = EventCalendar.NavigatedDate.Year;
                await LoadEventCollectionAsync(year, month);
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
            try
            {
                if (NavigatingToEvent)
                {
                    await SelectCalendar(currentlySelectedCalendarId);
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddSeconds(openDateStartTimeUnixSeconds);
                    EventCalendar.NavigatedDate = dateTime;
                    NavigatingToEvent = false;
                    SelectedDate = dateTime;
                    var tempDay = new Calendar<EventDay>();
                    tempDay.SelectedDates.Add(dateTime);
                    await LoadEventCollectionAsync(dateTime.Year, dateTime.Month);
                    EventCalendar_DaysUpdated(tempDay, null);
                    SelectedDates_CollectionChanged(null, null);
                    EventCalendar.Days.Single(d =>
                    {
                        return d.DateTime.Year == dateTime.Year && d.DateTime.Month == dateTime.Month && d.DateTime.Day == dateTime.Day;
                    }).IsSelected = true;
                }
                else
                {
                    await ExecuteLoadEventsAsync();
                    SelectedEvents.Clear();
                    EventCalendar.SelectedDates.Clear();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void SelectedDates_CollectionChanged(object _, NotifyCollectionChangedEventArgs __)
        {
            SelectedEvents.ReplaceRange(EventsBuffer.Where(x => EventCalendar.SelectedDates
                .Any(y => x.StartDate.Date == y.Date))
                .OrderByDescending(x => x.StartDate));
        }
        private void EventCalendar_DaysUpdated(object sender, EventArgs _)
        {
            var currSelectedDate = ((Calendar<EventDay>)sender).SelectedDates.FirstOrDefault();
            if (currSelectedDate != DateTime.MinValue)
            {
                _selectedDate = currSelectedDate;
            }

            foreach (var Day in EventCalendar.Days)
            {
                Day.Events.ReplaceRange(EventsBuffer.Where(x => x.StartDate.Date == Day.DateTime.Date));
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

                var color = SupportedColors.Where(c => Color.Parse(c).ToString() == e.Color.ToString()).Single();

                var yearlyEvent =
                    (!series.RepeatOnSun && !series.RepeatOnMon && !series.RepeatOnTues && !series.RepeatOnWed && !series.RepeatOnThurs && !series.RepeatOnFri && !series.RepeatOnSat && series.RepeatEveryWeek == 0);

                await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}&{nameof(EditSeriesViewModel.EntityType)}={series.EntityType}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}&{nameof(EditSeriesViewModel.Color)}={color}&{nameof(EditSeriesViewModel.ShouldNotify)}={e.ShouldNotify}&{nameof(EditSeriesViewModel.NumTimesNotified)}={e.NumTimesNotified}&{nameof(EditSeriesViewModel.YearlyEvent)}={yearlyEvent}");
            }
            else
            {
                var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
                var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();

                var color = SupportedColors.Where(c => Color.Parse(c).ToString() == e.Color.ToString()).Single();

                await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}&{nameof(EditEventViewModel.EntityType)}={e.EntityType}&{nameof(EditEventViewModel.Color)}={color}&{nameof(EditEventViewModel.ShouldNotify)}={e.ShouldNotify}&{nameof(EditEventViewModel.NumTimesNotified)}={e.NumTimesNotified}");
            }
        }
        private async Task SelectCalendar(CAL.Client.Models.Cal.Calendar calendar)
        {
            SelectedEvents.Clear();
            CurrentlySelectedCalendar = calendar;
            App.Current.Resources["ContentBackgroundColor"] = Color.Parse(CurrentlySelectedCalendar.Color);
            var month = EventCalendar.NavigatedDate.Month;
            var year = EventCalendar.NavigatedDate.Year;
            await LoadEventCollectionAsync(year, month);
        }
        private async Task SelectCalendar(string calendarId)
        {
            SelectedEvents.Clear();
            var calendars = await CalClientSingleton.GetCalendarsForUserAsync(new Guid(PreferencesManager.GetUserId()));
            var selectedCalendar = calendars.Calendars.FirstOrDefault(c => c.Id == new Guid(calendarId));
            App.Current.Resources["ContentBackgroundColor"] = Color.Parse(selectedCalendar.Color);
            CurrentlySelectedCalendar = selectedCalendar;
        }
    }
}
