using System;
using System.Collections.Generic;
using CAL.Client.Interfaces;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class EventsResponse : BaseResponse
    {
        public List<Event> Events { get; set; } = new ();
    }
}
