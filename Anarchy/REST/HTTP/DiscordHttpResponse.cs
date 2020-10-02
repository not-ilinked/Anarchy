using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordHttpResponse
    {
        public int StatusCode { get; private set; }
        public JToken Object { get; private set; }

        public DiscordHttpResponse(int statusCode, string content)
        {
            StatusCode = statusCode;
            Object = JToken.Parse(content);
        }


        public T Deserialize<T>()
        {
            return Object.ToObject<T>();
        }

        public T DeserializeEx<T>() where T : ControllableEx
        {
            return Object.ToObjectEx<T>();
        }

        public List<T> DeserializeExArray<T>() where T : ControllableEx
        {
            return Deserialize<JArray>().DeserializeWithJson<T>();
        }

        public T ToChannel<T>() where T : DiscordChannel
        {
            return ((JObject)Object).ToChannel<T>();
        }

        public DiscordChannel ToChannel()
        {
            return ToChannel<DiscordChannel>();
        }


        public List<T> ToChannels<T>() where T : DiscordChannel
        {
            return ((JArray)Object).ToChannels<T>();
        }

        public List<DiscordChannel> ToChannels()
        {
            return ToChannels<DiscordChannel>();
        }
    }
}
