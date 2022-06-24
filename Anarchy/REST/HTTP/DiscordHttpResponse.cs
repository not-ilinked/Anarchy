using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class DiscordHttpResponse
    {
        public int StatusCode { get; private set; }
        public JToken Body { get; private set; }

        public DiscordHttpResponse(int statusCode, string content)
        {
            StatusCode = statusCode;
            if (content != null && content.Length != 0)
                Body = JToken.Parse(content);
        }


        public T Deserialize<T>()
        {
            return Body.ToObject<T>();
        }


        public T ParseDeterministic<T>()
        {
            return ((JObject)Body).ParseDeterministic<T>();
        }

        public List<T> MultipleDeterministic<T>()
        {
            return ((JArray)Body).MultipleDeterministic<T>();
        }
    }
}
