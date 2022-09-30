using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordInteraction : Controllable
    {
        public DiscordInteraction()
        {
            OnClientUpdated += (s, e) =>
            {
                Message.SetClient(Client);
                Data.SetClient(Client);

                if (Data.Resolved != null)
                {
                    if (Data.Resolved.Members != null)
                    {
                        foreach (var member in Data.Resolved.Members.Values)
                            member.GuildId = ulong.Parse(GuildId);
                    }

                    if (Data.Resolved.Roles != null)
                    {
                        foreach (var role in Data.Resolved.Roles.Values)
                            role.GuildId = ulong.Parse(GuildId);
                    }
                }


                if (Member != null)
                {
                    User = Member.User;
                    Member.SetClient(Client);
                    Member.GuildId = ulong.Parse(GuildId);
                }

                User.SetClient(Client);
            };
        }

        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("type")]
        public DiscordInteractionType Type { get; set; }

        [JsonProperty("data")]
        public DiscordInteractionData Data { get; set; }

        [JsonProperty("guild_id")]
        public string GuildId;

        public MinimalGuild Guild => GuildId != "" ? new MinimalGuild(ulong.Parse(GuildId)).SetClient(Client) : null;

        [JsonProperty("channel_id")]
        public string ChannelId;

        public MinimalTextChannel Channel => ChannelId != "" ? new MinimalTextChannel(ulong.Parse(ChannelId)).SetClient(Client) : null;

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("member")]
        public GuildMember Member { get; set; }

        [JsonProperty("user")]
        public DiscordUser User { get; set; }

        [JsonProperty("message")]
        public DiscordMessage Message { get; set; }

        public DateTimeOffset CreatedAt
        {
            get { return DateTimeOffset.FromUnixTimeMilliseconds((long) ((Id >> 22) + 1420070400000UL)); }
        }

        public Task RespondAsync(InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => Client.RespondToInteractionAsync(Id, Token, callbackType, properties);

        public DiscordInteraction Respond(InteractionCallbackType callbackType, InteractionResponseProperties properties = null, bool bRespond = false)
        {
            RespondAsync(callbackType, properties).GetAwaiter().GetResult();

            if (!bRespond)
            {
                return null;
            }

            return Client.GetRespondInteraction(ulong.Parse(ApplicationId), Token);
        }

        public Task ModifyResponseAsync(InteractionResponseProperties changes) => Client.ModifyInteractionResponseAsync(ulong.Parse(ApplicationId), Token, changes);
        public void ModifyResponse(InteractionResponseProperties changes) => ModifyResponseAsync(changes).GetAwaiter().GetResult();

    }
}
