using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateSeriesResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Guid? SeriesId { get; set; }
    }
}


