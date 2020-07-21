using Newtonsoft.Json;
using Discord.Gateway;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Discord.Media;

namespace Discord
{
    public static class JsonExtensions
    {
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Deserialize<T>(this Response response)
        {
            return response.ToString().Deserialize<T>();
        }

        internal static T Deserialize<T>(this GatewayResponse response)
        {
            return response.Data.ToString().Deserialize<T>();
        }

        internal static T Deserialize<T>(this DiscordMediaResponse response)
        {
            return response.Data.ToString().Deserialize<T>();
        }


        public static T DeserializeEx<T>(this string json) where T : ControllableEx
        {
            JObject obj = JObject.Parse(json);
            return obj.ToObject<T>().SetJson(obj);
        }

        public static T DeserializeEx<T>(this Response response) where T : ControllableEx
        {
            return response.ToString().DeserializeEx<T>();
        }

        internal static T DeserializeEx<T>(this GatewayResponse response) where T : ControllableEx
        {
            return response.Data.ToString().DeserializeEx<T>();
        }


        public static List<T> DeserializeExArray<T>(this Response response) where T : ControllableEx
        {
            return JArray.Parse(response.ToString()).DeserializeWithJson<T>();
        }

        public static List<T> DeserializeWithJson<T>(this JArray jArray) where T : ControllableEx
        {
            List<T> results = new List<T>();

            foreach (var child in jArray.Children<JObject>())
                results.Add(child.ToObject<T>().SetJson(child));

            return results;
        }
    }
}