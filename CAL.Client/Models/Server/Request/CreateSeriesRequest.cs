using Newtonsoft.Json;
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
        [JsonProperty("startson")]
        public DateTime StartsOn { get; set; }
        [JsonProperty("endson")]
        public DateTime EndsOn { get; set; }
        public TimeSpan SubEventStartTime { get; set; }
        public TimeSpan EndEventStartTime { get; set; }
    }
}


