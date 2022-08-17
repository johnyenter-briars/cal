using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Response;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CAL.Services
{
    public class EventsDataStore : ObservableCollection<Event>,  IDataStore<Event>
    {
        //IList<Event> events = new List<Event>();
        private readonly ICalClient CalClient;
        public EventsDataStore()
        {
            CalClient = DependencyService.Get<ICalClient>();
            //UpdateAuthentication();

            //Thread t = new Thread(async () =>
            //{
            //    while (true)
            //    {
            //        await FetchItems();
            //        await Task.Delay(10000);
            //    }
            //})
            //{
            //    IsBackground = true
            //};

            //t.Start();
        }
        public async Task<bool> AddItemAsync(Event e)
        {
            var success = await CalClient.CreateEventAsync(e.ToRequest());

            await RefreshItemsAsync();

            return success.StatusCode == 201;
        }
        public async Task<bool> UpdateItemAsync(Event e)
        {
            var success = await CalClient.UpdateEventAsync(e.ToUpdateRequest());

            await RefreshItemsAsync();

            return success.StatusCode == 201 || success.StatusCode == 200;
        }
        public async Task<bool> DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<Event> GetItemAsync(Guid id)
        {
            await RefreshItemsAsync();
            return await Task.FromResult(this.ToList().FirstOrDefault(s => s.Id == id));
        }
        public async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                UpdateAuthentication();
                await RefreshItemsAsync();
            }
            return this.ToList();
        }
        public async Task RefreshItemsAsync()
        {
            var newEvents = (await CalClient.GetEventsAsync()).Events;
            Clear();
            foreach (var e in newEvents)
            {
                Add(e);
                //events.Add(e);
            }
        }
        public async Task<IEnumerable<Event>> GetEventsForDayAsync(int day, bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await RefreshItemsAsync();
            }

            return this.ToList().Where(e => e.StartTime.Day == day);
        }
        public async Task<bool> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest)
        {
            var idk =  await CalClient.CreateSeriesAsync(createSeriesRequest);

            return true;
        }
        public void UpdateAuthentication(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                CalClient.UpdateSettings(   PreferencesManager.GetHostname(), 
                                            PreferencesManager.GetPort(), 
                                            PreferencesManager.GetApiKey(), 
                                            PreferencesManager.GetUserId());
            }
        }
        public ObservableCollection<Event> GetAsObservable()
        {
            return this;
        }
    }
}