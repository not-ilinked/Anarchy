using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Discord
{
    public class ChannelConverter<T> : JsonConverter where T : DiscordChannel
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
                return JObject.Load(reader).ToChannel<T>();
            else if (reader.TokenType == JsonToken.StartArray)
                return Activator.CreateInstance(objectType, JsonUtils.ToChannels<T>(JArray.Load(reader)));
            else
                throw new JsonException("Invalid use of ChannelConverter");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class ChannelConverter : ChannelConverter<DiscordChannel>
    { }
}
