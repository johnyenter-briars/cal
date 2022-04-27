using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAL.Services
{
    public class EventsDataStore : IDataStore<Event>
    {
        IList<Event> events;
        private readonly ICalClient CalClient;
        public EventsDataStore()
        {
            CalClient = CalClientFactory.GetNewCalClient();
            UpdateAuthentication();
        }
        public async Task<bool> AddItemAsync(Event e)
        {
            var success = await CalClient.CreateEventAsync(e.ToRequest());

            await FetchEvents();

            return success.StatusCode == 201;
        }
        public async Task<bool> UpdateItemAsync(Event e)
        {
            throw new NotImplementedException();
            //var oldItem = events.Where((Event arg) => arg.Id == e.Id).FirstOrDefault();
            //events.Remove(oldItem);
            //events.Add(e);

            //return await Task.FromResult(true);
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
            await FetchEvents();
            return await Task.FromResult(events.FirstOrDefault(s => s.Id == id));
        }
        public async Task<IEnumerable<Event>> GetItemsAsync(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await FetchEvents();
            }
            return events;
        }
        private async Task FetchEvents()
        {
            //TODO: dynamically update based on new data - rather than wipe everything out
            events = (await CalClient.GetEventsAsync()).Events;
        }
        public async Task<IEnumerable<Event>> GetEventsForDayAsync(int day, bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                await FetchEvents();
            }

            return events.Where(e => e.StartTime.Day == day);
        }
        public void UpdateAuthentication(bool forceRefresh = true)
        {
            if (forceRefresh)
            {
                CalClient.UpdateSettings(PreferencesManager.GetHostname(), PreferencesManager.GetPort(), PreferencesManager.GetApiKey());
            }
        }
    }
}