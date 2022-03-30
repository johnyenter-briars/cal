using System.Collections.Generic;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class CalUserResponse
    {
        public CalUser User { get; set; }
        public int Status_code { get; set; }
        public string Message { get; set; }
    }
}

