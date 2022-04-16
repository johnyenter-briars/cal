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
            return await PostRequest<CreateCalUserResponse, CreateCalUserRequest>(createCalUserRequest, "caluser");
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

            return await PostRequest<CreateEventResponse, CreateEventRequest>(createEventRequest, "event");
        }
        public async Task<CreateSeriesResponse> CreateSeriesAsync(CreateSeriesRequest createSeriesRequest)
        {
            return await PostRequest<CreateSeriesResponse, CreateSeriesRequest>(createSeriesRequest, "series");
        }
        public async Task<CalUserResponse> GetCalUserAsync(Guid id)
        {
            return await GetRequest<CalUserResponse>($"caluser/{id}");
        }
        public async Task<EventsResponse> GetEventsAsync()
        {
            return await GetRequest<EventsResponse>($"event");
        }
        public async Task<SeriesResponse> GetSeriesAsync(Guid id)
        {
            return await GetRequest<SeriesResponse>($"series/{id}");
        }
        private bool ValidateRequest(CreateEventRequest request)
        {
            return request.StartTime.Kind == DateTimeKind.Utc &&
                    request.EndTime.Kind == DateTimeKind.Utc &&
                    request.CalUserId != null;
        }
        private async Task<T> GetRequest<T>(string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://{_hostName}:{_port}/api/" + path);
            request.Headers.Accept.Clear();
            request.Headers.Add("x-api-key", _apiKey);

            var clientResponse = await _httpClient.SendAsync(request, CancellationToken.None);

            if (clientResponse.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());
                return response;
            }
            else
            {
                throw new Exception($"Failure to get record {clientResponse.ReasonPhrase}");
            }
        }

        private async Task<T> PostRequest<T, V>(V requestObject, string path) where T : IResponse
        {
            var request = new StringContent(JsonConvert.SerializeObject(requestObject, JsonSettings), Encoding.UTF8, "application/json");

            var clientResponse = await _httpClient.PostAsync(path, request);

            var response = JsonConvert.DeserializeObject<T>(await clientResponse.Content.ReadAsStringAsync());

            if (clientResponse.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                throw new Exception($"Failure to create record: E: {response.GetMessage()}");
            }
        }
        public async Task<List<Event>> GetEventsForDayAsync(int dayOfCurrentMonth)
        {
            var currentMonth = DateTime.Now.Month;

            var allEvents = (await GetEventsAsync()).Events;

            var selectedEvents = allEvents.Where(e => e.StartTime.Month == currentMonth).ToList();

            return selectedEvents;
        }

        public void UpdateSettings(string hostname, int port, string apiKey)
        {
            _hostName = hostname;
            _port = port;
            _apiKey = apiKey;
        }
    }
}
