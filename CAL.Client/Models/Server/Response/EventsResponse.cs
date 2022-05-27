using System;
using System.Collections.Generic;
using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class EventsResponse : IResponse
    {
        public IList<Event> Events { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}
