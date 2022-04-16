using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAL.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemsAsync(Guid id);
        Task<T> GetEventAsync(Guid id);
        Task<IEnumerable<T>> GetItemAsync(bool forceRefresh = true);
        void UpdateAuthentication(bool forceRefresh = true);
    }
}
