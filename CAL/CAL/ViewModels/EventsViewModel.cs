using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        private Event _selectedEvent;
        public ObservableCollection<Event> Events { get; }
        public Command LoadEventsCommand { get; }
        public Command AddEventCommand { get; }
        public Command<Event> EventTapped { get; }

        public EventsViewModel ()
        {
            Title = "Events";
            Events = new ObservableCollection<Event>();
            LoadEventsCommand = new Command(async () => await ExecuteLoadEventsComand());

            EventTapped = new Command<Event>(OnEventSelected);

            AddEventCommand = new Command(OnAddEvent);
        }

        async Task ExecuteLoadEventsComand()
        {
            IsBusy = true;

            try
            {
                Events.Clear();
                var events = await EventDataStore.GetItemAsync(true);
                foreach (var e in events)
                {
                    Events.Add(e);
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Event SelectedItem
        {
            get => _selectedEvent;
            set
            {
                SetProperty(ref _selectedEvent, value);
                OnEventSelected(value);
            }
        }

        private async void OnAddEvent(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewEventPage));
        }

        async void OnEventSelected(Event e)
        {
            if (e == null)
                return;

            var idk = $"{nameof(EventDetailPage)}?{nameof(EventDetailViewModel.EventId)}={e.Id}";

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?{nameof(EventDetailViewModel.EventId)}={e.Id}");
        }
    }
}
