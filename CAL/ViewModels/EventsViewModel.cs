using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CAL.ViewModels
{
    public class EventsViewModel : BaseViewModel
    {
        bool fetchingNewPagedData = false;
        public ObservableCollection<Event> Events { get; }
        public Command LoadEventsCommand { get; }
        private int currentPage = 0;
        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            ToggleIsFetchingData();

            var events = await CalClientSingleton.GetEventsNameAsync(query);
            Events.Clear();
            foreach (var e in events.Events)
            {
                Events.Add(e);
            }

            ToggleIsFetchingData();
        });


        public EventsViewModel()
        {
            Events = new ObservableCollection<Event>();
            LoadEventsCommand = new Command(async () => await OnLoadEvents());
        }

        public async void Refresh()
        {
            //await OnLoadEvents();
        }
        private async Task OnLoadEvents(int page = 0)
        {
            var events = await CalClientSingleton.GetEventsPageAsync(page);
            Events.Clear();
            foreach (var e in events.Events)
            {
                Events.Add(e);
            }
        }
        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is Event e)
            {
                var eventStartTime = e.StartTime;
                var unixSeconds = ((DateTimeOffset)eventStartTime).ToUnixTimeSeconds();
                await Shell.Current.GoToAsync($@"//{nameof(CalendarPage)}?{nameof(CalendarViewModel.NavigatingToEvent)}={true}&{nameof(CalendarViewModel.OpenDateStartTimeUnixSeconds)}={unixSeconds}&{nameof(CalendarViewModel.CurrentlySelectedCalendarId)}={e.CalendarId}");
                //await Application.Current.;
                //await Shell.Current.GoToAsync($"//{nameof(CalendarPage)}");
            }
        }
        private bool fetchingData = false;
        public bool FetchingData
        {
            get { return fetchingData; }
            set { SetProperty(ref fetchingData, value); }
        }

        private bool notFetchingData = true;
        public bool NotFetchingData
        {
            get { return notFetchingData; }
            set { SetProperty(ref notFetchingData, value); }
        }
        private void ToggleIsFetchingData()
        {
            FetchingData = !FetchingData;
            NotFetchingData = !NotFetchingData;
        }
    }
}
