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
        public ICommand RemainingItemsThresholdReachedCommand => new Command(async () => await OnCollectionViewRemainingItemsThresholdReached());

        public EventsViewModel()
        {
            Events = new ObservableCollection<Event>();
            LoadEventsCommand = new Command(async () => await OnLoadEvents());
        }

        public async void Refresh()
        {
            await OnLoadEvents();
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
                //var startUnixTimeSeconds = ((DateTimeOffset)e.StartTime.ToUniversalTime()).ToUnixTimeSeconds();
                //var endUnixTimeSeconds = ((DateTimeOffset)e.EndTime.ToUniversalTime()).ToUnixTimeSeconds();
                //await Shell.Current.GoToAsync($@"{nameof(EditEventPage)}?{nameof(EditEventViewModel.StartTimeUnixSeconds)}={startUnixTimeSeconds}&{nameof(EditEventViewModel.EndTimeUnixSeconds)}={endUnixTimeSeconds}&{nameof(EditEventViewModel.Id)}={e.Id}&{nameof(EditEventViewModel.Name)}={e.Name}&{nameof(EditEventViewModel.Description)}={e.Description}");
            }
        }
        public async Task OnCollectionViewRemainingItemsThresholdReached()
        {
            if (fetchingNewPagedData) return;

            fetchingNewPagedData = true;

            currentPage++;

            var events = await CalClientSingleton.GetEventsPageAsync(currentPage);
            foreach (var e in events.Events)
            {
                Events.Add(e);
            }

            fetchingNewPagedData = false;
        }
    }
}
