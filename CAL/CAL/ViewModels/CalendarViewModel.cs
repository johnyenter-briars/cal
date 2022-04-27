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
        //public ICommand DayTappedCommand;
        public Command DayTappedCommand => new Command<DateTime>((date) => DayTapped(date));
        public Command EventTappedCommend => new Command<Event>(async (e) => await OnEventSelected(e));
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

            //DayTappedCommand = new Command<DateTime>(DayTapped);
            //EventTappedCommend = new Command<Event>(OnEventSelected);
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
            //saving this : )
            //var message = $"Received tap event from date: {date}";
            //await App.Current.MainPage.DisplayAlert("DayTapped", message, "Ok");

            if (!EventCollection.ContainsKey(date))
            {
                return;
            }

            Events.Clear();

            foreach (Event e in EventCollection[date])
            {
                Events.Add(e);
            }
        }
        async Task OnEventSelected(Event e)
        {
            if (e == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?{nameof(EventDetailViewModel.EventId)}={e.Id}");
        }
    }
}
