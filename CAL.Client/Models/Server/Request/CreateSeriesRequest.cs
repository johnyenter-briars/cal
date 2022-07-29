using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace CAL.Client.Models.Cal.Request
{
    using System;
    using CAL.Client.Models.Cal.Request.JsonTools;
    using Newtonsoft.Json;

    namespace JsonTools
    {
        /// <summary>
        /// TimeSpans are not serialized consistently depending on what properties are present. So this 
        /// serializer will ensure the format is maintained no matter what.
        /// </summary>
        public class TimespanConverter : JsonConverter<TimeSpan>
        {
            public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
            {
                writer.WriteValue(value.TotalSeconds);
            }

            public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                //TimeSpan parsedTimeSpan;
                //TimeSpan.TryParseExact((string)reader.Value, TimeSpanFormatString, null, out parsedTimeSpan);
                return TimeSpan.FromSeconds((int)reader.Value);
            }
        }
    }
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
        public DateTime EndsOn { get; set; }
        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan EventStartTime { get; set; }
        [JsonConverter(typeof(TimespanConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan EventEndTime { get; set; }
    }
}


