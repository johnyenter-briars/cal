using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CAL.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        public ObservableCollection<Event> Events { get; }
        public Command LoadEventsCommand { get; }
        public Command AddEventCommand { get; }
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

        public EventsViewModel ()
        {
            Title = "Events";
            Events = new ObservableCollection<Event>();
            LoadEventsCommand = new Command(async () => await ExecuteLoadEventsComand());

            AddEventCommand = new Command(OnAddEvent);
        }

        async Task ExecuteLoadEventsComand()
        {
            IsBusy = true;

            try
            {
                Events.Clear();
                var events = await EventDataStore.GetItemsAsync(true);
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
        }

        private async void OnAddEvent(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewEventPage));
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                await Shell.Current.GoToAsync($"{nameof(EventDetailPage)}?{nameof(EventDetailViewModel.EventId)}={e.Id}");
            }
        }
    }
}
