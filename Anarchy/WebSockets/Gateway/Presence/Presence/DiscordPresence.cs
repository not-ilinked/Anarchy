using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord.Gateway
{
    public class DiscordPresence : Controllable
    {
        public DiscordPresence()
        {
            OnClientUpdated += (sender, e) =>
            {
                Activities.SetClientsInList(Client);
            };
        }


        [JsonProperty("guild_id")]
        protected ulong? _guildId;


        public bool IsGuild
        {
            get { return _guildId.HasValue; }
        }


        [JsonProperty("user")]
        private readonly JObject _user;

        public ulong UserId
        {
            get { return _user["id"].ToObject<ulong>(); }
        }


        [JsonProperty("activities")]
        [JsonConverter(typeof(DeepJsonConverter<DiscordActivity>))]
        private readonly List<DiscordActivity> _activities;

        public IReadOnlyList<DiscordActivity> Activities
        {
            get { return _activities; }
        }


        [JsonProperty("status")]
        public UserStatus Status { get; private set; }


        [JsonProperty("client_status")]
        public ActiveSessionPlatforms ActivePlatforms { get; private set; }


        public override string ToString()
        {
            return UserId.ToString();
        }
    }
}
