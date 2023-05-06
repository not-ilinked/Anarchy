using System.Text.Json.Serialization;
using System.Threading.Tasks;


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
                            member.GuildId = _guildId.Value;
                    }

                    if (Data.Resolved.Roles != null)
                    {
                        foreach (var role in Data.Resolved.Roles.Values)
                            role.GuildId = _guildId.Value;
                    }
                }

                if (Member != null)
                {
                    User = Member.User;
                    Member.SetClient(Client);
                    Member.GuildId = _guildId.Value;
                }

                User.SetClient(Client);
            };
        }

        [JsonPropertyName("id")]
        public ulong Id { get; private set; }

        [JsonPropertyName("application_id")]
        public ulong ApplicationId { get; private set; }

        [JsonPropertyName("type")]
        public DiscordInteractionType Type { get; private set; }

        [JsonPropertyName("data")]
        public DiscordInteractionData Data { get; private set; }

        [JsonPropertyName("guild_id")]
        private readonly ulong? _guildId;

        public MinimalGuild Guild => _guildId.HasValue ? new MinimalGuild(_guildId.Value).SetClient(Client) : null;

        [JsonPropertyName("channel_id")]
        private readonly ulong? _channelId;

        public MinimalTextChannel Channel => _channelId.HasValue ? new MinimalTextChannel(_channelId.Value).SetClient(Client) : null;

        [JsonPropertyName("token")]
        public string Token { get; private set; }

        [JsonPropertyName("member")]
        public GuildMember Member { get; private set; }

        [JsonPropertyName("user")]
        public DiscordUser User { get; private set; }

        [JsonPropertyName("message")]
        public DiscordMessage Message { get; private set; }

        public Task RespondAsync(InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => Client.RespondToInteractionAsync(Id, Token, callbackType, properties);
        public void Respond(InteractionCallbackType callbackType, InteractionResponseProperties properties = null) => RespondAsync(callbackType, properties).GetAwaiter().GetResult();

        public Task ModifyResponseAsync(InteractionResponseProperties changes) => Client.ModifyInteractionResponseAsync(ApplicationId, Token, changes);
        public void ModifyResponse(InteractionResponseProperties changes) => ModifyResponseAsync(changes).GetAwaiter().GetResult();
    }
}
