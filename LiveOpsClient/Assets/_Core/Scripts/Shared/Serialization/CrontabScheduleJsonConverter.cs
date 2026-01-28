using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NCrontab;

namespace App.Shared.Serialization
{
    public sealed class CrontabScheduleJsonConverter : JsonConverter<CrontabSchedule>
    {
        public override CrontabSchedule ReadJson(
            JsonReader reader,
            Type objectType,
            CrontabSchedule existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.StartObject)
            {
                JToken.Load(reader);
                return null;
            }

            if (reader.TokenType == JsonToken.String)
            {
                var expression = (string)reader.Value;
                return string.IsNullOrWhiteSpace(expression) ? null : CrontabSchedule.Parse(expression);
            }

            throw new JsonSerializationException(
                $"Unexpected token {reader.TokenType} when parsing {nameof(CrontabSchedule)}.");
        }

        public override void WriteJson(JsonWriter writer, CrontabSchedule value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(value.ToString());
        }
    }
}
