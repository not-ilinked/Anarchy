using Newtonsoft.Json;
using Discord.Gateway;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace Discord
{
    internal static class JsonUtils
    {
        public static T Deserialize<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> DeserializeWithJson<T>(this JArray jArray) where T : ControllableEx
        {
            List<T> results = new List<T>();

            foreach (var child in jArray.Children<JObject>())
                results.Add(child.ToObject<T>().SetJson(child));

            return results;
        }

        public static T ToObjectEx<T>(this JToken obj) where T : ControllableEx
        {
            return obj.ToObject<T>().SetJson(obj);
        }


        public static T ToChannel<T>(this JObject obj) where T : DiscordChannel
        {
            var type = (ChannelType)obj.Value<int>("type");

            switch (type)
            {
                case ChannelType.DM:
                    return (T)(object)obj.ToObject<PrivateChannel>();
                case ChannelType.Group:
                    return (T)(object)obj.ToObject<DiscordGroup>();
                case ChannelType.Category:
                    return (T)(object)obj.ToObject<GuildChannel>();
                case ChannelType.Voice:
                    return (T)(object)obj.ToObject<VoiceChannel>();
                default:
                    if (type == ChannelType.News || type == ChannelType.Store || type == ChannelType.Text)
                        return (T)(object)obj.ToObject<TextChannel>();
                    else
                        throw new InvalidCastException("Could not determine the channel's type");
            }
        }

        public static DiscordChannel ToChannel(this JObject obj)
        {
            return obj.ToChannel<DiscordChannel>();
        }


        public static List<T> ToChannels<T>(this JArray objects) where T : DiscordChannel
        {
            List<T> channels = new List<T>();

            foreach (JObject obj in objects)
                channels.Add(obj.ToChannel<T>());

            return channels;
        }

        public static List<DiscordChannel> ToChannels(this JArray objects)
        {
            return ToChannels<DiscordChannel>(objects);
        }
    }
}