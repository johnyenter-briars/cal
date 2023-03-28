using CAL.Client.Models;
using CAL.Client.Models.Cal;
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
        Task<CreateCalUserResponse> CreateCalUserAsync(CreateCalUserRequest createCalUserRequest);
        Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest);
        Task<CalendarsResponse> GetCalendarsForUserAsync(Guid calUserId);
        Task<EventsResponse> GetEventsAsync();
        Task<EventsResponse> GetEventsPageAsync(int page);
        Task<EventsResponse> GetEventsNameAsync(string page);
        /// <summary>
        /// Ok yea this method isnt totally accurate - cause dates are stored as UTC on server. So a DateTime that's on the first of the month in Local, might still be in the previous month in UTC.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<EventsResponse> GetEventsAsync(int year, int month);
        Task<CalUserResponse> GetCalUserAsync(Guid id);
        Task<SeriesResponse> GetSeriesAsync(Guid id);
        Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth);
        Task<UpdateEntityResponse> UpdateEventAsync(UpdateEventRequest updateEventRequest);
        Task<UpdateEntityResponse> UpdateSeriesAsync(UpdateSeriesRequest updateEventRequest);
        Task<DeletedEntityResponse> DeleteEntityAsync(Guid entityId, EntityType entityType);
    }
}

