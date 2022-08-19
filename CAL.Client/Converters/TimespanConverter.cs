using Newtonsoft.Json;
using System;

namespace CAL.Client.Converters
{
    internal class TimespanConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Seconds);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var idk = reader.Value.GetType();
            var foo = (Int64)reader.Value;
            return TimeSpan.FromSeconds((Int64)reader.Value);
        }
    }
}
