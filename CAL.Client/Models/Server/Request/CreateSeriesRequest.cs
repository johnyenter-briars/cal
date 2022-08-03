using CAL.Client.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace CAL.Client.Models.Cal.Request
{
    public class CreateSeriesRequest
    {
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
    }
}


