using CAL.Client.Interfaces;
using CAL.Client.Models;
using CAL.Client.Models.Cal;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Request;
using CAL.Client.Models.Server.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CAL.Client
{
    internal class CalClient : ICalClient
    {
        private string _apiKey = "";
        private string _userId = "";
        private string _hostName = "";
        private int _port = -1;
        private static readonly HttpClient _httpClient = new HttpClient();
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
        }
        public async Task<CreateCalUserResponse> CreateCalUserAsync(CreateCalUserRequest createCalUserRequest)
        {
            return await CalServerRequest<CreateCalUserRequest, CreateCalUserResponse>(createCalUserRequest, "caluser", HttpMethod.Post);
        }
        public async Task<CreateEventResponse> CreateEventAsync(CreateEventRequest createEventRequest)
        {
            if (!ValidateRequest(createEventRequest))
            {
                return new CreateEventResponse
                {
                    StatusCode = 400,
                    Message = "Bad Request",
                };
            }

            return await CalServerRequest<CreateEventRequest, CreateEventResponse>(createEventRequest, "event", HttpMethod.Post);
        }
        public async Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest)
        {
            return await CalServerRequest<CreateSeriesRequest, CreateSeriesResponse>(createSeriesRequest, "series", HttpMethod.Post);
        }
        public async Task<CalUserResponse> GetCalUserAsync(Guid id)
        {
            return await CalServerRequest<CalUserResponse>($"caluser/{id}", HttpMethod.Get);
        }
        public async Task<EventsResponse> GetEventsAsync()
        {
            return await CalServerRequest<EventsResponse>($"event", HttpMethod.Get);
        }
        public async Task<SeriesResponse> GetSeriesAsync(Guid id)
        {
            return await CalServerRequest<SeriesResponse>($"series/{id}", HttpMethod.Get);
        }
        public async Task<UpdateEventResponse> UpdateEventAsync(UpdateEventRequest updateEventRequest)
        {
            if (!ValidateRequest(updateEventRequest))
            {
                return new UpdateEventResponse
                {
                    StatusCode = 400,
                    Message = "Bad Request",
                };
            }

            return await CalServerRequest<UpdateEventRequest, UpdateEventResponse>(updateEventRequest, "event", HttpMethod.Put);
        }
        private bool ValidateRequest(IValidatable request)
        {
            request.CalUserId = null;
            return request.StartTime.Kind == DateTimeKind.Utc &&
                    request.EndTime.Kind == DateTimeKind.Utc &&
                    request.CalUserId != null;
        }
        private async Task<TResponse> CalServerRequest<TResponse>(string path, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, $"http://{_hostName}:{_port}/api/" + path);
            request.Headers.Accept.Clear();
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("x-user-id", _userId);

            return await SendRequest<TResponse>(request);
        }
        private async Task<TResponse> CalServerRequest<TRequest, TResponse>(TRequest requestObject, string path, HttpMethod httpMethod) where TResponse : IResponse
        {
            var request = new HttpRequestMessage(httpMethod, $"http://{_hostName}:{_port}/api/" + path);
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
                return JsonConvert.DeserializeObject<TResponse>(await clientResponse.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception($"Failure to complete action. Reason phrase: {clientResponse.ReasonPhrase}, Raw response: {await clientResponse.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth)
        {
            var currentMonth = DateTime.Now.Month;

            var allEvents = (await GetEventsAsync()).Events;

            var selectedEvents = allEvents.Where(e => e.StartTime.Month == currentMonth).ToList();

            return selectedEvents;
        }
        public void UpdateSettings(string hostname, int port, string apiKey, string userId)
        {
            _hostName = hostname;
            _port = port;
            _apiKey = apiKey;
            _userId = userId;
        }

    }
}
