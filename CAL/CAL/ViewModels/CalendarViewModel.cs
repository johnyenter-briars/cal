using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public EventCollection EventCollection { get; set; } = new EventCollection();
        public DateTime _selectedDate;
        public Guid CurrentlySelectedCalendarId { get; set; } 
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        //public Dictionary<string, EventCollection> Calendars = new Dictionary<string, EventCollection>();
        public CalendarViewModel(Guid defaultCalendarId)
        {
            CurrentlySelectedCalendarId = defaultCalendarId;
            Title = "Calendar";
            _selectedDate = DateTime.Now;
            Task.Run(async () => await ExecuteLoadEventsAsync());
        }
        private async Task SelectCalendar(object calendarId)
        {
            if (calendarId is Guid id)
            {
                CurrentlySelectedCalendarId = id;
                var events = (await EventDataStore.GetItemsAsync()).Where(e => e.CalendarId == id).ToList();
                await LoadEventCollection(events, EventCollection);
            }
        }
        private async Task LoadEventCollection(IList<Event> events, EventCollection eventCollection)
        {
            eventCollection.Clear();
            foreach (var e in events)
            {
                if (eventCollection.ContainsKey(e.StartTime))
                {
                    var listOfEvents = ((List<Event>)eventCollection[e.StartTime]);

                    var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

                    if (eventWithSameId != null)
                    {
                        listOfEvents.Remove(eventWithSameId);
                        listOfEvents.Add(e);
                    }
                }
                else
                {
                    eventCollection.Add(e.StartTime, new List<Event> { e });
                }
            }
        }
        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                //var events = await EventDataStore.GetItemsAsync();
                //foreach (var e in events)
                //{
                //    if (EventCollection.ContainsKey(e.StartTime))
                //    {
                //        var listOfEvents = ((List<Event>)EventCollection[e.StartTime]);

                //        var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

                //        if (eventWithSameId != null)
                //        {
                //            listOfEvents.Remove(eventWithSameId);
                //            listOfEvents.Add(e);
                //        }
                //    }
                //    else
                //    {
                //        EventCollection.Add(e.StartTime, new List<Event> { e });
                //    }
                //}
                var events = (await EventDataStore.GetItemsAsync()).Where(e => e.CalendarId == CurrentlySelectedCalendarId).ToList();
                await LoadEventCollection(events, EventCollection);
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
            await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}");
        }
        private async void OnAddSeries()
        {
            var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
            var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddHours(1)).ToUnixTimeSeconds();
            await Shell.Current.GoToAsync($@"{nameof(EditSeriesPage)}?{nameof(EditSeriesViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditSeriesViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}");
        }
        private async void Refresh()
        {
            await EventDataStore.RefreshItemsAsync();
            await ExecuteLoadEventsAsync();
        }
        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
                var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
                await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}");
            }
        }
    }
}
