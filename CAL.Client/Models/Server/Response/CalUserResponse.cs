using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class CalUserResponse : IResponse
    {
        public CalUser User { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string GetMessage() => Message;
        public int GetStatusCode() => StatusCode;
    }
}

