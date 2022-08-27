using CAL.Client;
using CAL.Client.Models.Cal;
using CAL.Models;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace CAL.ViewModels
{
    internal class CalendarViewModel : BaseViewModel
    {
        public Command AddEventCommand => new Command(OnAddEvent);
        public Command AddSeriesCommand => new Command(OnAddSeries);
        public Command RefreshEventsCommand => new Command(Refresh);
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        public ICommand SelectCalendarCommand => new Command(async (item) => await SelectCalendar(item));
        public string SelectedCalendar { get; set; }
        public EventCollection Events { get; } = new EventCollection();
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
            Task.Run(async () => await ExecuteLoadEventsAsync());
        }
        private async Task SelectCalendar(object calendar)
        {
            if (calendar is Calendar cal)
            {
                CurrentlySelectedCalendar = cal;
                var events = (await CalClientSingleton.GetEventsAsync()).Events.Where(e => e.CalendarId == CurrentlySelectedCalendar.Id).ToList();
                LoadEventCollection(events, Events);
            }
        }
        private void LoadEventCollection(IList<Event> events, EventCollection eventCollection)
        {
            eventCollection.Clear();

            foreach (var e in events)
            {
                if (eventCollection.ContainsKey(e.StartTime))
                {
                    var listOfEvents = ((DayEventCollection<Event>)eventCollection[e.StartTime]);
                    listOfEvents.Add(e);

                    //var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

                    //if (eventWithSameId != null)
                    //{
                    //    throw new ApplicationException("ireally dont think this should ever happen");
                    //}
                }
                else
                {
                    ColorTypeConverter converter = new ColorTypeConverter();
                    Color color = (Color)(converter.ConvertFromInvariantString(CurrentlySelectedCalendar.Color));

                    var dayEventCollection = new DayEventCollection<Event>(color, color) { e };
                    eventCollection.Add(e.StartTime, dayEventCollection);
                }
            }
        }
        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                var events = (await CalClientSingleton.GetEventsAsync()).Events.Where(e => e.CalendarId == CurrentlySelectedCalendar.Id).ToList();
                LoadEventCollection(events, Events);
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
            await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
        }
        private async void OnAddSeries()
        {
            var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
            var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddDays(1)).ToUnixTimeSeconds();
            await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.CurrentlySelectedCalendar)}={CurrentlySelectedCalendar.Id}");
        }
        private async void Refresh()
        {
            //await EventDataStore.RefreshItemsAsync();
            await ExecuteLoadEventsAsync();
        }
        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                if (e.SeriesId != null)
                {
                    var series = (await DependencyService.Get<ICalClient>().GetSeriesAsync((Guid)e.SeriesId)).Series;
                    //var startsOnUtc = series.StartsOn.ToString();
                    //var startsOnLocal = series.StartsOn.ToLocalTime().ToString();
                    //var subEventStartTime = series.EventStartTime.ToString();
                    var endsOnUtc = series.EndsOn.ToString();
                    var endsOnLocal = series.EndsOn.ToLocalTime().ToString();
                    var subEventEndtime = series.EventEndTime.ToString();
                    var startUnixTimeSeconds = ((DateTimeOffset)series.StartsOn.Add(series.EventStartTime).ToUniversalTime()).ToUnixTimeSeconds();
                    var endUnixTimeSeconds = ((DateTimeOffset)series.EndsOn.Add(series.EventEndTime).ToUniversalTime()).ToUnixTimeSeconds();
                    await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditSeriesViewModel.Id)}={series.Id}&{nameof(EditSeriesViewModel.Name)}={series.Name}&{nameof(EditSeriesViewModel.Description)}={series.Description}&{nameof(EditSeriesViewModel.RepeatEveryWeek)}={series.RepeatEveryWeek}&{nameof(EditSeriesViewModel.RepeatOnMon)}={series.RepeatOnMon}&{nameof(EditSeriesViewModel.RepeatOnTues)}={series.RepeatOnTues}&{nameof(EditSeriesViewModel.RepeatOnWed)}={series.RepeatOnWed}&{nameof(EditSeriesViewModel.RepeatOnThurs)}={series.RepeatOnThurs}&{nameof(EditSeriesViewModel.RepeatOnFri)}={series.RepeatOnFri}&{nameof(EditSeriesViewModel.RepeatOnSat)}={series.RepeatOnSat}&{nameof(EditSeriesViewModel.RepeatOnSun)}={series.RepeatOnSun}");
                }
                else
                {
                    var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
                    var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
                    await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}");
                }
            }
        }
    }
}
