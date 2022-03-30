using CAL.Client.Models.Server.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Cal
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
        public int CalUserId { get; set; }
        public int SeriesId { get; set; }
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
