using CAL.Client.Models.Cal;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Server.Response
{
    public class CalendarsResponse
    {
        public IList<Calendar> Calendars { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
