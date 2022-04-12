using CAL.Client.Models.Cal;
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
        public ICommand DayTappedCommand => new Command<DateTime>(async (date) => await DayTapped(date));

        public ObservableCollection<Event> Events { get; }
        public EventCollection EventCollection { get; }

        public Command<Event> EventTapped { get; }
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
            Task.Run(async () => await ExecuteLoadEventsComand());
            EventCollection = new EventCollection();
        }
        async Task ExecuteLoadEventsComand()
        {
            IsBusy = true;

            try
            {
                Events.Clear();
                var events = await DataStore.GetEventsAsync();
                foreach (var e in events)
                {
                    Events.Add(e);
                    var idk = EventCollection.Keys;
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
        private async Task DayTapped(DateTime date)
        {
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
    }
}
