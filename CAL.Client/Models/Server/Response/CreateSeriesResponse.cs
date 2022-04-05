using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateSeriesResponse : IResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? SeriesId { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}


