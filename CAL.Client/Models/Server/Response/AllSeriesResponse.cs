using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;
using System.Collections.Generic;

namespace CAL.Client.Models.Server.Response
{
    public class AllSeriesResponse : IResponse 
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Series> Series { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}



