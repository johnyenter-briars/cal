using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CAL.Services
{
    public class EventsDataStore : ObservableCollection<Event>,  IDataStore<Event>
    {
        IList<Event> events = new List<Event>();
        private readonly ICalClient CalClient;
        public EventsDataStore()
        {
            CalClient = CalClientFactory.GetNewCalClient();
            UpdateAuthentication();

            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    await FetchItems();
                    await Task.Delay(10000);
                }
            })
            {
                IsBackground = true
            };

            t.Start();
        }
        public async Task<bool> AddItemAsync(Event e)
        {
            var success = await CalClient.CreateEventAsync(e.ToRequest());

            await FetchItems();

            return success.StatusCode == 201;
        }
        public async Task<bool> UpdateItemAsync(Event e)
        {
            var success = await CalClient.UpdateEventAsync(e.ToUpdateRequest());

            await FetchItems();

            return success.StatusCode == 201 || success.StatusCode == 200;
        }
        public async Task<bool> DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
            //var oldItem = events.Where((Event arg) => arg.Id == id).FirstOrDefault();
            //events.Remove(oldItem);

            //return await Task.FromResult(true);
        }
        public async Task<Event> GetItemAsync(Guid id)
        {
            await FetchItems();
            return await Task.FromResult(events.FirstOrDefault(s => s.Id == id));
        }
        public async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                UpdateAuthentication();
                await FetchItems();
            }
            return events;
        }
        private async Task FetchItems()
        {
            var newEvents = (await CalClient.GetEventsAsync()).Events;
            Clear();
            foreach (var e in newEvents)
            {
                Add(e);
                events.Add(e);
            }
        }
        public async Task<IEnumerable<Event>> GetEventsForDayAsync(int day, bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await FetchItems();
            }

            return events.Where(e => e.StartTime.Day == day);
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