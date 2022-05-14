using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;

namespace CAL.ViewModels
{
    internal class CalendarViewModel : BaseViewModel
    {
        public Command DayTappedCommand => new Command<DateTime>((date) => DayTapped(date));
        public Command AddEventCommand { get; }
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
            Events = new ObservableCollection<Event>();
            _selectedDate = DateTime.Now;
            Task.Run(async () => await ExecuteLoadEventsAsync());
            EventCollection = new EventCollection();
            AddEventCommand = new Command(OnAddEvent);
        }
        private async Task ExecuteLoadEventsAsync()
        {
            IsBusy = true;

            try
            {
                Events.Clear();
                EventCollection.Clear();
                var events = await EventDataStore.GetItemsAsync();
                foreach (var e in events)
                {
                    Events.Add(e);
                    if (EventCollection.ContainsKey(e.StartTime))
                    {
                        ((List<Event>)EventCollection[e.StartTime]).Add(e);
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
        private async void DayTapped(DateTime date)
        {
            await ExecuteLoadEventsAsync();
        }
        private async void OnAddEvent(object obj)
        {
            var unixTimeSeconds = ((DateTimeOffset)SelectedDate).ToUnixTimeSeconds();
            await Shell.Current.GoToAsync($"{nameof(NewEventPage)}?{nameof(NewEventViewModel.StartTimeUnixSeconds)}={unixTimeSeconds}");
        }
        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                var startAsUtc = e.StartTime.ToUniversalTime();
                var startUnixTimeSeconds = ((DateTimeOffset)startAsUtc).ToUnixTimeSeconds();
                var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime).ToUnixTimeSeconds();
                await Shell.Current.GoToAsync($@"{nameof(NewEventPage)}?{nameof(NewEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(NewEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(NewEventViewModel.Id)}={e.Id}&{nameof(NewEventViewModel.Name)}={e.Name}&{nameof(NewEventViewModel.Description)}={e.Description}");
            }
        }
    }
}
