using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAL.Services
{
    public interface IEventDataStore<T>
    {
        Task<bool> AddEventAsync(T item);
        Task<bool> UpdateEventAsync(T item);
        Task<bool> DeleteEventsAsync(Guid id);
        Task<T> GetEventAsync(Guid id);
        Task<IEnumerable<T>> GetEventsAsync(bool forceRefresh = true);
        Task<IEnumerable<T>> GetEventsForDayAsync(int day, bool forceRefresh = true);
    }
}
