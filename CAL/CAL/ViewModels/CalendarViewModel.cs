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
        //public Command DayTappedCommand => new Command<DateTime>((date) => DayTapped(date));
        public Command AddEventCommand => new Command(OnAddEvent);
        public Command RefreshEventsCommand => new Command(Refresh);
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        public ObservableCollection<Event> Events { get; }
        public EventCollection EventCollection { get; }
        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public CalendarViewModel()
        {
            Title = "Calendar";
            Events = EventDataStore.GetAsObservable();
            _selectedDate = DateTime.Now;
            Task.Run(async () => await ExecuteLoadEventsAsync());
            EventCollection = new EventCollection();
        }
        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                var events = await EventDataStore.GetItemsAsync();
                foreach (var e in events)
                {
                    if (e.Name == "a")
                    {
                        var x = 5;
                    }
                    if (EventCollection.ContainsKey(e.StartTime))
                    {
                        var listOfEvents = ((List<Event>)EventCollection[e.StartTime]);

                        var eventWithSameId = listOfEvents.Where(temp => e.ShouldReplace(temp)).SingleOrDefault();

                        if (eventWithSameId != null)
                        {
                            listOfEvents.Remove(eventWithSameId);
                            listOfEvents.Add(e);
                        }
                    }
                    else
                    {
                        EventCollection.Add(e.StartTime, new List<Event> { e });
                    }
                }
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
