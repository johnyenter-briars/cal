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
	//TODO: fix events page - CalClient needs to be an observable
	public class EventsViewModel : BaseViewModel
	{
		public ObservableCollection<Event> Events { get; }
		public Command LoadEventsCommand { get; }
		public Command AddEventCommand { get; }
		public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));

		public EventsViewModel()
		{
			Title = "Events";
			//Events = EventDataStore.GetAsObservable();
			LoadEventsCommand = new Command(async () => await ExecuteLoadEventsComand());

			AddEventCommand = new Command(OnAddEvent);

		}

		async Task<IEnumerable<Event>> GetNewData()
		{
			//var events = await EventDataStore.GetItemsAsync();

			return null;
		}
		async Task ExecuteLoadEventsComand()
		{
			IsBusy = true;
			//await EventDataStore.RefreshItemsAsync();
			IsBusy = false;
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private async void OnAddEvent(object obj)
		{
			await Shell.Current.GoToAsync(nameof(EditEventPage));
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
