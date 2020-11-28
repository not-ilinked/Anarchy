using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public class DiscordHttpResponse
    {
        public int StatusCode { get; private set; }
        public JToken Object { get; private set; }

        public DiscordHttpResponse(int statusCode, string content)
        {
            StatusCode = statusCode;
            if (content != null && content.Length != 0)
                Object = JToken.Parse(content);
        }


        public T Deserialize<T>()
        {
            return Object.ToObject<T>();
        }


        public T ParseDeterministic<T>()
        {
            return ((JObject)Object).ParseDeterministic<T>();
        }

        public List<T> MultipleDeterministic<T>()
        {
            return ((JArray)Object).MultipleDeterministic<T>();
        }
    }
}
