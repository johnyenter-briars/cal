using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAL.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddEventAsync(T item);
        Task<bool> UpdateEventAsync(T item);
        Task<bool> DeleteEventsAsync(int id);
        Task<T> GetEventAsync(int id);
        Task<IEnumerable<T>> GetEventsAsync(bool forceRefresh = false);
    }
}
