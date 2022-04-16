using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Request;
using CAL.Client.Models.Server.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client
{
    public interface ICalClient
    {
        void UpdateSettings(string hostname, int port, string apiKey);
        Task<CreateEventResponse> CreateEventAsync(CreateEventRequest createEventRequest);
        Task<CreateCalUserResponse> CreateCalUserAsync(CreateCalUserRequest createCalUserRequest);
        Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest);
        Task<EventsResponse> GetEventsAsync();
        Task<CalUserResponse> GetCalUserAsync(Guid id);
        Task<SeriesResponse> GetSeriesAsync(Guid id);
        Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth);
    }
}

