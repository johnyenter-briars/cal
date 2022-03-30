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
    public static class CalClient
    {
        private static readonly string _serverUrl = "http://192.168.0.10:8000/api/";
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri(_serverUrl)
        };

        //public CalClient()
        //{
        //    _httpClient = new HttpClient();
        //    _httpClient.BaseAddress = new Uri(_serverUrl);
        //}

        public static async Task<bool> CreateEvent(CreateEventRequest createEventRequest)
        {
            if (!ValidateRequest(createEventRequest))
            {
                return false;
            }

            var request = new StringContent(JsonConvert.SerializeObject(createEventRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("event", request);

            return response.IsSuccessStatusCode;
        }
        public static async Task<IList<Event>> GetEvents()
        {
            var response = await _httpClient.GetAsync("event");

            if (response.IsSuccessStatusCode)
            {
                var eventsResponse = JsonConvert.DeserializeObject<EventsResponse>(await response.Content.ReadAsStringAsync());

                return eventsResponse.Events;
            }
            else
            {
                throw new Exception("Idk what to do");
            }
        }
        private static bool ValidateRequest(CreateEventRequest request)
        {
            return request.StartTime.Kind == DateTimeKind.Utc;
        }
    }
}
