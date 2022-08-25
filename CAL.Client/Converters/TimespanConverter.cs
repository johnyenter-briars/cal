using Newtonsoft.Json;
using System;

namespace CAL.Client.Converters
{
    internal class TimespanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Seconds + (value.Minutes * 60) + (value.Hours * 60 * 60));
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeSpan.FromSeconds((Int64)reader.Value);
        }
    }
}
