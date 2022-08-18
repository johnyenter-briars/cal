using CAL.Client.Models.Cal;
using CAL.Models;
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
using Xamarin.Plugin.Calendar.Interfaces;
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
        public Guid CurrentlySelectedCalendarId { get; set; }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public CalendarViewModel(Guid defaultCalendarId)
        {
            CurrentlySelectedCalendarId = defaultCalendarId;
            Title = "Calendar";
            _selectedDate = DateTime.Now;
            Task.Run(async () => await ExecuteLoadEventsAsync());
            //Events = new EventCollection
            //{
            //    [DateTime.Now.AddDays(-3)] = new List<Event>(GenerateEvents(10, "Cool")),
            //    [DateTime.Now.AddDays(-6)] = new DayEventCollection<Event>(Color.Purple, Color.Purple)
            //        {
            //            new Event { Name = "Cool event1", Description = "This is Cool event1's description!", StartTime = new DateTime().ToUniversalTime() },
            //            new Event { Name = "Cool event2", Description = "This is Cool event2's description!", StartTime = new DateTime().ToUniversalTime() }
            //        }
            //};

            ////Adding a day with a different dot color
            //Events.Add(DateTime.Now.AddDays(-2), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Blue, EventIndicatorSelectedColor = Color.Blue });
            //Events.Add(DateTime.Now.AddDays(-4), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Green, EventIndicatorSelectedColor = Color.White });
            //Events.Add(DateTime.Now.AddDays(-5), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Orange, EventIndicatorSelectedColor = Color.Orange });

            //// with add method
            //Events.Add(DateTime.Now.AddDays(-1), new List<Event>(GenerateEvents(5, "Cool")));

            // with indexer
            //Events[DateTime.Now] = new List<Event>(GenerateEvents(2, "Boring"));
        }
        private async Task SelectCalendar(object calendarId)
        {
            if (calendarId is Guid id)
            {
                CurrentlySelectedCalendarId = id;
                var events = (await EventDataStore.GetItemsAsync()).Where(e => e.CalendarId == id).ToList();
                LoadEventCollection(events, Events);
            }
        }
        private void LoadEventCollection(IList<Event> events, EventCollection eventCollection)
        {
            eventCollection.Clear();
            //var Events = eventCollection;
            //Events = new EventCollection
            //{
            //    [DateTime.Now.AddDays(-3)] = new List<Event>(GenerateEvents(10, "Cool")),
            //    [DateTime.Now.AddDays(-6)] = new DayEventCollection<Event>(Color.Purple, Color.Purple)
            //    {
            //        new Event { Name = "Cool event1", Description = "This is Cool event1's description!", StartTime = new DateTime().ToUniversalTime() },
            //        new Event { Name = "Cool event2", Description = "This is Cool event2's description!", StartTime = new DateTime().ToUniversalTime() }
            //    }
            //};

            ////Adding a day with a different dot color
            //Events.Add(DateTime.Now.AddDays(-2), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Blue, EventIndicatorSelectedColor = Color.Blue });
            //Events.Add(DateTime.Now.AddDays(-4), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Green, EventIndicatorSelectedColor = Color.White });
            //Events.Add(DateTime.Now.AddDays(-5), new DayEventCollection<Event>(GenerateEvents(10, "Cool")) { EventIndicatorColor = Color.Orange, EventIndicatorSelectedColor = Color.Orange });

            //// with add method
            //Events.Add(DateTime.Now.AddDays(-1), new List<Event>(GenerateEvents(5, "Cool")));

            //// with indexer
            //Events[DateTime.Now] = new List<Event>(GenerateEvents(2, "Boring"));

            foreach (var e in events)
            {
                if (eventCollection.ContainsKey(e.StartTime))
                {
                    var listOfEvents = ((DayEventCollection<Event>)eventCollection[e.StartTime]);

                    var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

                    if (eventWithSameId != null)
                    {
                        throw new ApplicationException("ireally dont think this should ever happen");
                        //listOfEvents.Remove(eventWithSameId);
                        //listOfEvents.Add(e);
                    }
                }
                else
                {
                    var dayEventCollection = new DayEventCollection<Event>(Color.Orange, Color.Orange) { e };
                    eventCollection.Add(e.StartTime, dayEventCollection);
                }
            }
            //eventCollection[DateTime.Now.AddDays(-5)] = new DayEventCollection<Event>(Color.Blue, Color.Blue)
            //    {
            //        new Event
            //        {
            //            Name = "test",
            //            Description = "idk",
            //            StartTime = DateTime.Now.AddDays(-5).ToUniversalTime(),
            //            EndTime = DateTime.Now.AddDays(-5).ToUniversalTime(),
            //        },
            //        new Event
            //        {
            //            Name = "test",
            //            Description = "idk",
            //            StartTime = DateTime.Now.AddDays(-5).ToUniversalTime(),
            //            EndTime = DateTime.Now.AddDays(-5).ToUniversalTime(),
            //        }
            //    };
        }
        private IEnumerable<Event> GenerateEvents(int count, string name)
        {
            return Enumerable.Range(1, count).Select(x => new Event
            {
                Name = $"{name} event{x}",
                Description = $"This is {name} event{x}'s description!",
                StartTime = new DateTime(2000, 1, 1, (x * 2) % 24, (x * 3) % 60, 0).ToUniversalTime()
            });
        }

        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                var events = (await EventDataStore.GetItemsAsync()).Where(e => e.CalendarId == CurrentlySelectedCalendarId).ToList();
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
