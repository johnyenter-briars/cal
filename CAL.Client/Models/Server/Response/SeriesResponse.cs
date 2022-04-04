using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class SeriesResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Series Series { get; set; }
    }
}



