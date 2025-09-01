using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Server.Request;
using CAL.Client.Models.Server.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CAL.Client
{
    public class CalClient : ICalClient
    {
        private string _apiKey = "";
        private string _userId = "";
        private string _hostName = "";
        private int _port = -1;
        private readonly HttpClient _httpClient;
        private static readonly JsonSerializerSettings JsonSettings =
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                };

        public CalClient()
        {
            HttpClientHandler insecureHandler = GetInsecureHandler();
            _httpClient = new HttpClient(insecureHandler);
        }
        public async Task<CreateCalUserResponse> CreateCalUserAsync(CreateCalUserRequest createCalUserRequest)
        {
            return await CalServerRequest<CreateCalUserRequest, CreateCalUserResponse>(createCalUserRequest, "caluser", HttpMethod.Post);
        }
        public async Task<CreateEventResponse> CreateEventAsync(CreateEventRequest createEventRequest)
        {
            return await CalServerRequest<CreateEventRequest, CreateEventResponse>(createEventRequest, "event", HttpMethod.Post);
        }
        public async Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest)
        {
            var createSeriesResponse = await CalServerRequest<CreateSeriesRequest, CreateSeriesResponse>(createSeriesRequest, "series", HttpMethod.Post);

            var startsOn = createSeriesRequest.StartsOn;
            var endsOn = createSeriesRequest.EndsOn;

            if (ShouldAddOnToday(createSeriesRequest, startsOn.DayOfWeek))
            {
                var _ = await CreateEventAsync(createSeriesRequest.CreateSubEventRequest(startsOn, (Guid)createSeriesResponse.SeriesId));
            }
            int currentWeek = 0;

            var currentDay = startsOn;
            while (true)
            {
                var dayToAdd = GetNextDayToAdd(createSeriesRequest, currentDay);

                if (!DatesAreInTheSameWeek(currentDay, dayToAdd))
                {
                    currentWeek++;
                }

                currentDay = dayToAdd;

                if (currentWeek % createSeriesRequest.RepeatEveryWeek != 0)
                {
                    continue;
                }

                if (dayToAdd > endsOn)
                {
                    break;
                }

                var _ = await CreateEventAsync(createSeriesRequest.CreateSubEventRequest(dayToAdd, (Guid)createSeriesResponse.SeriesId));
            }

            return createSeriesResponse;
        }
        public async Task<CalUserResponse> GetCalUserAsync(Guid id)
        {
            return await CalServerRequest<CalUserResponse>($"caluser/{id}", HttpMethod.Get);
        }
        public async Task<EventsResponse> GetEventsAsync()
        {
            var eventsResponse = await CalServerRequest<EventsResponse>($"event", HttpMethod.Get);
            var seriesResponse = await CalServerRequest<AllSeriesResponse>($"series", HttpMethod.Get);

            foreach (var e in eventsResponse.Events)
            {
                if (e.SeriesId != null && e.SeriesId != Guid.Empty)
                {
                    e.SeriesName = seriesResponse.Series.FirstOrDefault(s => s.Id == e.SeriesId)?.Name ?? throw new Exception($"No series found with id: {e.SeriesId}");
                }
            }

            return eventsResponse;
        }
        public async Task<EventsResponse> GetEventsPageAsync(int page)
        {
            var eventsResponse = await CalServerRequest<EventsResponse>($"event/page/{page}", HttpMethod.Get);
            var seriesResponse = await CalServerRequest<AllSeriesResponse>($"series", HttpMethod.Get);

            foreach (var e in eventsResponse.Events)
            {
                if (e.SeriesId != null && e.SeriesId != Guid.Empty)
                {
                    e.SeriesName = seriesResponse.Series.FirstOrDefault(s => s.Id == e.SeriesId)?.Name ?? throw new Exception($"No series found with id: {e.SeriesId}");
                }
            }

            return eventsResponse;
        }
        public async Task<SeriesResponse> GetSeriesAsync(Guid id)
        {
            return await CalServerRequest<SeriesResponse>($"series/{id}", HttpMethod.Get);
        }
        public async Task<UpdateEntityResponse> UpdateEventAsync(UpdateEventRequest updateEventRequest)
        {
            if (!ValidateRequest(updateEventRequest))
            {
                return new UpdateEntityResponse
                {
                    StatusCode = 400,
                    Message = "Bad Request",
                };
            }

            return await CalServerRequest<UpdateEventRequest, UpdateEntityResponse>(updateEventRequest, "event", HttpMethod.Put);
        }
        private bool ValidateRequest(IValidatable request)
        {
            return request.Validate();
        }
        private async Task<TResponse> CalServerRequest<TResponse>(string path, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, $"https://{_hostName}/cal/api/" + path);
            request.Headers.Accept.Clear();
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("x-user-id", _userId);

            return await SendRequest<TResponse>(request);
        }
        private async Task<TResponse> CalServerRequest<TRequest, TResponse>(TRequest requestObject, string path, HttpMethod httpMethod)
                where TResponse : IResponse, new()
                where TRequest : IValidatable
        {
            if (!requestObject.Validate())
            {
                return (TResponse)new TResponse().SetMessage("BadRequest").SetStatusCode(400);
            }

            var request = new HttpRequestMessage(httpMethod, $"https://{_hostName}/cal/api/" + path);
            request.Headers.Accept.Clear();
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("x-user-id", _userId);

            request.Content = new StringContent(JsonConvert.SerializeObject(requestObject, JsonSettings), Encoding.UTF8, "application/json");

            return await SendRequest<TResponse>(request);
        }
        private async Task<TResponse> SendRequest<TResponse>(HttpRequestMessage request)
        {
            var clientResponse = await _httpClient.SendAsync(request, CancellationToken.None);

            if (clientResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(await clientResponse.Content.ReadAsStringAsync()) ?? throw new NullReferenceException("JsonConvert.DeserializeObject<TResponse>");
            }
            else
            {
                var message = $"Failure to complete action. Reason phrase: {clientResponse.ReasonPhrase}, Raw response: {await clientResponse.Content.ReadAsStringAsync()}, Endpoint: {request.RequestUri}";
                throw new Exception(message);
            }
        }
        public async Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth)
        {
            var currentMonth = DateTime.Now.Month;

            var allEvents = (await GetEventsAsync()).Events;

            var selectedEvents = allEvents.Where(e => e.StartTime.Month == currentMonth).ToList();

            return selectedEvents;
        }
        public ICalClient UpdateSettings(string hostname, int port, string apiKey, string userId)
        {
            _hostName = hostname;
            _port = port;
            _apiKey = apiKey;
            _userId = userId;
            return this;
        }
        //https://stackoverflow.com/questions/6346119/compute-the-datetime-of-an-upcoming-weekday
        private static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            if (daysToAdd == 0)
            {
                daysToAdd = 7;
            }
            return start.AddDays(daysToAdd);
        }
        private bool ShouldAddOnToday(CreateSeriesRequest request, DayOfWeek dayOfWeek)
        {
            bool returnVal = false;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    returnVal = request.RepeatOnMon;
                    break;
                case DayOfWeek.Tuesday:
                    returnVal = request.RepeatOnTues;
                    break;
                case DayOfWeek.Wednesday:
                    returnVal = request.RepeatOnWed;
                    break;
                case DayOfWeek.Thursday:
                    returnVal = request.RepeatOnThurs;
                    break;
                case DayOfWeek.Friday:
                    returnVal = request.RepeatOnFri;
                    break;
                case DayOfWeek.Saturday:
                    returnVal = request.RepeatOnSat;
                    break;
                case DayOfWeek.Sunday:
                    returnVal = request.RepeatOnSun;
                    break;
            }

            return returnVal;
        }
        private DateTime GetNextDayToAdd(CreateSeriesRequest request, DateTime currentDay)
        {
            var daysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };

            void CircularIncrement(ref int i, int count)
            {
                if (i == count - 1)
                    i = 0;
                else
                    i++;
            }

            for (var index = daysOfWeek.IndexOf(currentDay.DayOfWeek) == daysOfWeek.Count - 1 ? 0 : daysOfWeek.IndexOf(currentDay.DayOfWeek) + 1;
                index > -1;
                CircularIncrement(ref index, daysOfWeek.Count))
            {
                var dayOfWeek = daysOfWeek[index];
                if (ShouldAddOnToday(request, dayOfWeek))
                {
                    var nextDayToAdd = GetNextWeekday(currentDay, dayOfWeek);
                    var week = DatesAreInTheSameWeek(currentDay, nextDayToAdd) ? 0 : 1;

                    return nextDayToAdd;
                }
            }

            throw new ApplicationException("We should never get here");
        }

        public async Task<CreateCalendarResponse> CreateCalendarAsync(CreateCalendarRequest createCalendarRequest)
        {
            if (!ValidateRequest(createCalendarRequest))
            {
                return new CreateCalendarResponse
                {
                    StatusCode = 400,
                    Message = "Bad Request",
                };
            }

            return await CalServerRequest<CreateCalendarRequest, CreateCalendarResponse>(createCalendarRequest, "calendar", HttpMethod.Post);
        }

        public async Task<CalendarsResponse> GetCalendarsForUserAsync(Guid calUserId)
        {
            return await CalServerRequest<CalendarsResponse>($"calendar/user/{calUserId}", HttpMethod.Get);
        }

        public async Task<DeletedEntityResponse> DeleteEntityAsync(Guid entityId, EntityType entityType)
        {
            var lowerCase = entityType.ToString().ToLower();
            return await CalServerRequest<DeletedEntityResponse>($"{lowerCase}/{entityId}", HttpMethod.Delete) as DeletedEntityResponse;
        }

        public async Task<UpdateEntityResponse> UpdateSeriesAsync(UpdateSeriesRequest updateEventRequest)
        {
            var _ = await DeleteEntityAsync(updateEventRequest.Id, EntityType.Series);

            var created = await CreateSeriesAsync(updateEventRequest.ToCreateSeriesRequest());

            return new UpdateEntityResponse
            {
                Message = created.Message,
                EntityId = created.SeriesId,
                StatusCode = created.StatusCode,
            };
        }
        //https://stackoverflow.com/questions/25795254/check-if-a-datetime-is-in-same-week-as-other-datetime
        private bool DatesAreInTheSameWeek(DateTime date1, DateTime date2)
        {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        }

        public async Task<EventsResponse> GetEventsAsync(int year, int month)
        {
            var eventsResponse = await CalServerRequest<EventsResponse>($"event/{year}/{month}", HttpMethod.Get);
            var seriesResponse = await CalServerRequest<AllSeriesResponse>($"series", HttpMethod.Get);

            foreach (var e in eventsResponse.Events)
            {
                e.SeriesName = seriesResponse.Series.FirstOrDefault(s => s.Id == e.SeriesId)?.Name;
            }

            return eventsResponse;
        }
        public async Task<EventsResponse> GetEventsNameAsync(string name)
        {
            var eventsResponse = await CalServerRequest<EventsResponse>($"event/{name}", HttpMethod.Get);
            var seriesResponse = await CalServerRequest<AllSeriesResponse>($"series", HttpMethod.Get);

            foreach (var e in eventsResponse.Events)
            {
                e.SeriesName = seriesResponse.Series.FirstOrDefault(s => s.Id == e.SeriesId)?.Name;
            }

            return eventsResponse;
        }

        public async Task<CreateSeriesResponse> CreateYearlySeriesAsync(CreateSeriesRequest createSeriesRequest, int numYearsInFuture)
        {
            var createSeriesResponse = await CalServerRequest<CreateSeriesRequest, CreateSeriesResponse>(
                createSeriesRequest, "series", HttpMethod.Post);

            for (var yearsToAdd = 0; yearsToAdd <= numYearsInFuture; yearsToAdd++)
            {
                var startsOn = createSeriesRequest.StartsOn.AddYears(yearsToAdd);

                var _ = await CreateEventAsync(createSeriesRequest.CreateSubEventRequest(
                    startsOn, (Guid)createSeriesResponse.SeriesId));
            }

            return createSeriesResponse;
        }

        public async Task<UpdateEntityResponse> UpdateYearlySeriesAsync(UpdateSeriesRequest updateSeriesRequest, int numYearsInFuture)
        {
            var _ = await DeleteEntityAsync(updateSeriesRequest.Id, EntityType.Series);

            var created = await CreateYearlySeriesAsync(updateSeriesRequest.ToCreateSeriesRequest(), numYearsInFuture);

            return new UpdateEntityResponse
            {
                Message = created.Message,
                EntityId = created.SeriesId,
                StatusCode = created.StatusCode,
            };
        }
        //https://stackoverflow.com/questions/71047509/trust-anchor-for-certification-path-not-found-in-a-net-maui-project-trying-t
        private HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) =>
                    {
                        return true;
                        //if (cert.Issuer.Equals("CN=localhost"))
                        //    return true;
                        //return errors == System.Net.Security.SslPolicyErrors.None;
                    };
            return handler;
        }
    }
}
