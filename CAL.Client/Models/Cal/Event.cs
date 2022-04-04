using CAL.Client.Models.Server.Request;
using System;

namespace CAL.Client.Models.Cal
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public Guid CalUserId { get; set; }
        public Guid? SeriesId { get; set; }
        public CreateEventRequest ToRequest()
        {
            return new CreateEventRequest
            {
                StartTime = StartTime,
                Name = Name,
            };
        }
    }
}
