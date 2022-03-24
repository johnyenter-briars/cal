using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Response
{
    public class EventsResponse
    {
        public IList<Event> Events { get; set; }
        public int Status_code { get; set; }
        public string Message { get; set; }
    }
}
