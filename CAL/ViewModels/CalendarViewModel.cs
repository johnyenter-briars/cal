using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XCalendar.Maui;

namespace CAL.ViewModels
{

    internal class CalendarViewModel : BaseViewModel
    {
        //public Command DayTappedCommand => new Command<DateTime>((date) => DayTapped(date));
        public Command AddEventCommand => new Command(OnAddEvent);
        //public Command RefreshEventsCommand => new Command(Refresh);
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        //public ObservableRangeCollection<Event> Events { get; }
        //public EventCollection EventCollection { get; }
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public List<Color> Colors { get; } = new List<Color>() { Microsoft.Maui.Graphics.Colors.Red, Microsoft.Maui.Graphics.Colors.Orange, Microsoft.Maui.Graphics.Colors.Yellow, Color.FromArgb("#FF00A000"), Microsoft.Maui.Graphics.Colors.Blue, Color.FromArgb("#FF8010E0") };
        public EventDayResolver EventDayResolver { get; set; } = new EventDayResolver();
        public ObservableRangeCollection<Event> Events { get; } = new ObservableRangeCollection<Event>()
        {
            new Event() { Name = "Bowling",
                            Description = "Bowling with friends",

            },
        };
        public ObservableRangeCollection<DateTime> SelectedDates { get; } = new ObservableRangeCollection<DateTime>();
        public ObservableRangeCollection<Event> SelectedEvents { get; } = new ObservableRangeCollection<Event>();

        public CalendarViewModel()
        {
            var random = new Random();
            Title = "Calendar";
            foreach (Event Event in Events)
            {
                Event.StartTime = DateTime.Today.AddDays(random.Next(-20, 21)).AddSeconds(random.Next(86400));
            }

            EventDayResolver.Events = Events;

            SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;
        }
        private void SelectedDates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedEvents.ReplaceRange(Events.Where(x => SelectedDates.Any(y => x.StartTime.Date == y.Date)).OrderByDescending(x => x.DateTime));
        }

        private async Task ExecuteLoadEventsAsync()
        {
            //IsBusy = true;

            //try
            //{
            //    var events = await EventDataStore.GetItemsAsync();
            //    foreach (var e in events)
            //    {
            //        if (e.Name == "a")
            //        {
            //            var x = 5;
            //        }
            //        if (EventCollection.ContainsKey(e.StartTime))
            //        {
            //            var listOfEvents = ((List<Event>)EventCollection[e.StartTime]);

            //            var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

            //            if (eventWithSameId != null)
            //            {
            //                listOfEvents.Remove(eventWithSameId);
            //                listOfEvents.Add(e);
            //            }
            //        }
            //        else
            //        {
            //            EventCollection.Add(e.StartTime, new List<Event> { e });
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
            //finally
            //{
            //    IsBusy = false;
            //}
        }
        private async void OnAddEvent()
        {
            var startUnixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
            var endUnixTimeSeconds = ((DateTimeOffset)SelectedDate.AddHours(1)).ToUnixTimeSeconds();
            await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}");
        }
        private async Task Refresh()
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
    public class EventDayResolver : ICalendarDayResolver
    {
        #region Properties
        public IEnumerable<Event> Events { get; set; }
        #endregion

        #region Methods
        public ICalendarDay CreateDay(DateTime? DateTime)
        {
            EventDay EventDay = new EventDay();
            UpdateDay(EventDay, DateTime);
            return EventDay;
        }
        public void UpdateDay(ICalendarDay Day, DateTime? DateTime)
        {
            EventDay EventDay = (EventDay)Day;
            EventDay.Events.ReplaceRange(Events.Where(x => x.DateTime.Date == DateTime?.Date));
            EventDay.DateTime = DateTime;
        }
        #endregion
    }
    public interface ICalendarDayResolver
    {
        ICalendarDay CreateDay(DateTime? DateTime);
        void UpdateDay(ICalendarDay Day, DateTime? DateTime);
    }
    public interface ICalendarDayResolver
    {
        ICalendarDay CreateDay(DateTime? DateTime);
        void UpdateDay(ICalendarDay Day, DateTime? DateTime);
    }


}
