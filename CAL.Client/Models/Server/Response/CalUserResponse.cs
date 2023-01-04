using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class CalUserResponse : BaseResponse
    {
        public CalUser User { get; set; } = new ();
    }
}

