using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using Discord.Gateway;

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
                    { (int)ChannelType.GuildNewsThread, typeof(TextChannel) },
                    { (int)ChannelType.GuildPublicThread, typeof(TextChannel) },
                    { (int)ChannelType.GuildPrivateThread, typeof(TextChannel) },
                    { (int)ChannelType.Stage, typeof(StageChannel) },
                    { (int)ChannelType.Event, typeof(GuildChannel) }
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
                    { (int)ActivityType.CustomStatus, typeof(CustomStatusActivity) },
                    { (int)ActivityType.IdkWhatThisIs, typeof(DiscordActivity) }
                }
            },
            {
                typeof(MessageComponent),
                new Dictionary<int, Type>()
                {
                    { (int)MessageComponentType.Row, typeof(RowComponent) },
                    { (int)MessageComponentType.Button, typeof(ButtonComponent) },
                    { (int)MessageComponentType.Select, typeof(SelectMenuComponent) }

                }
            }
        };
    }

    internal class DeepJsonConverter<T> : JsonConverter<T>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
                return JsonSerializer.Deserialize<T>(ref reader, options);
            else if (reader.TokenType == JsonTokenType.StartArray)
                return JsonSerializer.Deserialize<List<T>>(ref reader, options)[0];
            else
                throw new JsonException("Invalid use of DeepJsonConverter");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}