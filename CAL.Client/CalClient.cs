using CAL.Client.Models;
using CAL.Client.Models.Request;
using CAL.Client.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CAL.Client
{
    public class CalClient
    {
        private const string _serverUrl = "http://192.168.0.10:8000/api/";
        private HttpClient _httpClient;

        public CalClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_serverUrl);
        }

        public async Task<bool> CreateEvent(string name)
        {
            var request = new StringContent("");
            var response = await _httpClient.PostAsync("event/" + name, request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<IList<Event>> GetEvents()
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
    }
}
