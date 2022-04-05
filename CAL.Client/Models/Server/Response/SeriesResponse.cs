using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class SeriesResponse : IResponse 
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Series Series { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}



