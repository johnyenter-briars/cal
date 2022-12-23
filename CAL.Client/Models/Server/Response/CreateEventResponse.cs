using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateEventResponse : BaseResponse
    {
        public Guid? EventId { get; set; }
    }
}
