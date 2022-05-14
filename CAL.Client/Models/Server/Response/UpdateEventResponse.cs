using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class UpdateEventResponse : IResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? EventId { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}
