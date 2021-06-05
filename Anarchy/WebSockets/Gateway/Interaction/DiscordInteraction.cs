using Newtonsoft.Json;
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
            };
        }

        [JsonProperty("id")]
        public ulong Id { get; private set; }

        [JsonProperty("application_id")]
        public ulong ApplicationId { get; private set; }

        [JsonProperty("type")]
        public DiscordInteractionType Type { get; private set; }

        [JsonProperty("data")]
        public DiscordInteractionData Data { get; private set; }

        [JsonProperty("guild_id")]
        private readonly ulong? _guildId;

        public MinimalGuild Guild => _guildId.HasValue ? new MinimalGuild(_guildId.Value).SetClient(Client) : null;

        [JsonProperty("channel_id")]
        private readonly ulong? _channelId;

        public MinimalTextChannel Channel => _channelId.HasValue ? new MinimalTextChannel(_channelId.Value).SetClient(Client) : null;

        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("member")]
        public GuildMember Member { get; private set; }

        [JsonProperty("user")]
        public DiscordUser User { get; private set; }

        [JsonProperty("message")]
        public DiscordMessage Message { get; private set; }

        public Task RespondAsync(InteractionCallbackType callbackType, InteractionCallbackProperties properties = null) => Client.RespondToInteractionAsync(Id, Token, callbackType, properties);
        public void Respond(InteractionCallbackType callbackType, InteractionCallbackProperties properties = null) => RespondAsync(callbackType, properties).GetAwaiter().GetResult();

        public Task ModifyResponseAsync(InteractionCallbackProperties changes) => Client.ModifyInteractionResponseAsync(ApplicationId, Token, changes);
        public void ModifyResponse(InteractionCallbackProperties changes) => ModifyResponseAsync(changes).GetAwaiter().GetResult();
    }
}
