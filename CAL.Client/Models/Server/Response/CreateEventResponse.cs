using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateEventResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? EventId { get; set; }
    }
}
