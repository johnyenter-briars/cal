using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Server.Request
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid CalUserId { get; set; }
        public Guid? SeriesId { get; set; }
    }
}
