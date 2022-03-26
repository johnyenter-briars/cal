using CAL.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAL.Services
{
    public class EventsDataStore : IDataStore<Event>
    {
        readonly List<Event> events;

        public EventsDataStore()
        {
            events = new List<Event>()
            {
                new Event { Id = 1, Name = "Sixth item", Time = DateTime.Now },
                new Event { Id = 2, Name = "Sixth item", Time = DateTime.Now },
                new Event { Id = 3, Name = "Sixth item", Time = DateTime.Now },
                new Event { Id = 4, Name = "Sixth item", Time = DateTime.Now },
                new Event { Id = 5, Name = "Sixth item", Time = DateTime.Now },
                new Event { Id = 6, Name = "Sixth item", Time = DateTime.Now },
            };
        }

        public async Task<bool> AddEventAsync(Event e)
        {
            events.Add(e);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateEventAsync(Event e)
        {
            var oldItem = events.Where((Event arg) => arg.Id == e.Id).FirstOrDefault();
            events.Remove(oldItem);
            events.Add(e);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteEventsAsync(int id)
        {
            var oldItem = events.Where((Event arg) => arg.Id == id).FirstOrDefault();
            events.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Event> GetEventAsync(int id)
        {
            return await Task.FromResult(events.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(events);
        }
    }
}