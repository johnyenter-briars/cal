using CAL.Client;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
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
        }

        public async Task<bool> AddEventAsync(Event e)
        {
            var success = await CalClient.CreateEventAsync(e.ToRequest());

            await FetchEvents();

            return success;
        }

        public async Task<bool> UpdateEventAsync(Event e)
        {
            throw new NotImplementedException();
            //var oldItem = events.Where((Event arg) => arg.Id == e.Id).FirstOrDefault();
            //events.Remove(oldItem);
            //events.Add(e);

            //return await Task.FromResult(true);
        }

        public async Task<bool> DeleteEventsAsync(int id)
        {
            throw new NotImplementedException();
            //var oldItem = events.Where((Event arg) => arg.Id == id).FirstOrDefault();
            //events.Remove(oldItem);

            //return await Task.FromResult(true);
        }

        public async Task<Event> GetEventAsync(int id)
        {
            await FetchEvents();
            return await Task.FromResult(events.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(bool forceRefresh = false)
        {
            await FetchEvents();
            return await Task.FromResult(events);
        }

        private async Task FetchEvents()
        {
            //TODO: dynamically update based on new data - rather than wipe everything out
            events = await CalClient.GetEventsAsync();
        }
    }
}