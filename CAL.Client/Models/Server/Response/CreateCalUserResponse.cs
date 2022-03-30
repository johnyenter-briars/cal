
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class CreateCalUserResponse
    {
        public int Status_code { get; set; }
        public string Message { get; set; }
        public int CalUserId { get; set; }
    }
}

