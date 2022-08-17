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
        ICalClient UpdateSettings(string hostname, int port, string apiKey, string userId);
        Task<CreateEventResponse> CreateEventAsync(CreateEventRequest createEventRequest);
        Task<CreateCalendarResponse> CreateCalendarAsync(CreateCalendarRequest createCalendarRequest);
        Task<UpdateEventResponse> UpdateEventAsync(UpdateEventRequest updateEventRequest);
        Task<CreateCalUserResponse> CreateCalUserAsync(CreateCalUserRequest createCalUserRequest);
        Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest);
        Task<CalendarsResponse> GetCalendarsForUserAsync(Guid calUserId);
        Task<EventsResponse> GetEventsAsync();
        Task<CalUserResponse> GetCalUserAsync(Guid id);
        Task<SeriesResponse> GetSeriesAsync(Guid id);
        Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth);
    }
}

