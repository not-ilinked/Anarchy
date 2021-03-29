using Discord.Gateway;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Discord
{
    internal static class DeepJsonConverter
    {
        public static readonly Dictionary<Type, Dictionary<int, Type>> RecognizedTypes = new Dictionary<Type, Dictionary<int, Type>>()
        {
            {
                typeof(DiscordChannel),
                new Dictionary<int, Type>()
                {
                    { (int)ChannelType.DM, typeof(PrivateChannel) },
                    { (int)ChannelType.Group, typeof(DiscordGroup) },
                    { (int)ChannelType.Category, typeof(GuildChannel) },
                    { (int)ChannelType.Voice, typeof(VoiceChannel) },
                    { (int)ChannelType.Text, typeof(TextChannel) },
                    { (int)ChannelType.News, typeof(TextChannel) },
                    { (int)ChannelType.Store, typeof(TextChannel) },
                    { (int)ChannelType.Stages, typeof(VoiceChannel) }
                }
            },
            {
                typeof(PaymentMethod),
                new Dictionary<int, Type>()
                {
                    { (int)PaymentMethodType.Card, typeof(CardPaymentMethod) },
                    { (int)PaymentMethodType.PayPal, typeof(PayPalPaymentMethod) }
                }
            },
            {
                typeof(DiscordInvite),
                new Dictionary<int, Type>()
                {
                    { (int)InviteType.Guild, typeof(GuildInvite) },
                    { (int)InviteType.Group, typeof(DiscordInvite) }
                }
            },
            {
                typeof(DiscordWebhook),
                new Dictionary<int, Type>()
                {
                    { (int)DiscordWebhookType.Default, typeof(DiscordDefaultWebhook) },
                    { (int)DiscordWebhookType.ChannelFollower, typeof(DiscordCrosspostWebhook) }
                }
            },
            {
                typeof(DiscordActivity),
                new Dictionary<int, Type>()
                {
                    { (int)ActivityType.Streaming, typeof(DiscordActivity) },
                    { (int)ActivityType.Watching, typeof(DiscordActivity) },
                    { (int)ActivityType.Game, typeof(DiscordGameActivity) },
                    { (int)ActivityType.Listening, typeof(DiscordListeningActivity) },
                    { (int)ActivityType.CustomStatus, typeof(CustomStatusActivity) }
                }
            }
        };
    }

    internal class DeepJsonConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
                return JObject.Load(reader).ParseDeterministic<T>();
            else if (reader.TokenType == JsonToken.StartArray)
                return Activator.CreateInstance(objectType, JArray.Load(reader).MultipleDeterministic<T>());
            else
                throw new JsonException("Invalid use of DeepJsonConverter");
        }

        public override void WriteJson(JsonWriter writer,  object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
