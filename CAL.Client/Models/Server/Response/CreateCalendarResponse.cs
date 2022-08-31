using CAL.Client.Interfaces;
using System;

namespace CAL.Client.Models.Server.Response
{
    public class CreateCalendarResponse : BaseResponse
    {
        public Guid? CalendarId { get; set; }
    }
}
