using CAL.Client.Converters;
using CAL.Client.Models.Cal.Request;
using CAL.Client.Models.Server.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Cal
{
    public class Series
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RepeatEveryWeek { get; set; }
        public bool RepeatOnMon { get; set; }
        public bool RepeatOnTues { get; set; }
        public bool RepeatOnWed { get; set; }
        public bool RepeatOnThurs { get; set; }
        public bool RepeatOnFri { get; set; }
        public bool RepeatOnSat { get; set; }
        public bool RepeatOnSun { get; set; }
        public DateTime StartsOn { get; set; }
        public DateTime EndsOn { get; set; }
        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan EventStartTime { get; set; } 
        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan EventEndTime { get; set; } 
        public Guid CalUserId { get; set; }
        public Guid CalendarId { get; set; }
        public CreateSeriesRequest ToRequest()
        {
            return new CreateSeriesRequest 
            {
                Name = Name,
                RepeatEveryWeek = RepeatEveryWeek,
                RepeatOnMon = RepeatOnMon,
                RepeatOnTues = RepeatOnTues,
                RepeatOnFri = RepeatOnFri,
                RepeatOnSat = RepeatOnSat,
                RepeatOnSun = RepeatOnSun,
                EndsOn = EndsOn,
                StartsOn = StartsOn,
            };
        }
    }
}

