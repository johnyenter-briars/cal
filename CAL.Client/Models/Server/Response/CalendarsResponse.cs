using CAL.Client.Models.Cal;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAL.Client.Models.Server.Response
{
    public class CalendarsResponse: BaseResponse
    {
        public List<Calendar> Calendars { get; set; } = new ();
    }
}
