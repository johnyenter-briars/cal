using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class CalUserResponse
    {
        public CalUser User { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

