﻿using System;
using System.Collections.Generic;
using CAL.Client.Models.Cal;

namespace CAL.Client.Models.Server.Response
{
    public class EventsResponse
    {
        public IList<Event> Events { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
