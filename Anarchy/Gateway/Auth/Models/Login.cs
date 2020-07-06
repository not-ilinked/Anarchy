using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    /// <summary>
    /// A successful gateway login
    /// </summary>
    internal class Login : ControllableEx, IDisposable
    {
        internal Login()
        {
            OnClientUpdated += (sender, e) =>
            {
                User.SetClient(Client);
                Guilds.SetClientsInList(Client);
                Relationships.SetClientsInList(Client);
                PrivateChannels.SetClientsInList(Client);
                ClientGuildSettings.SetClientsInList(Client);
                ConnectedAccounts.SetClientsInList(Client);
                Presences.SetClientsInList(Client);
            };
            JsonUpdated += (sender, json) =>
            {
                Guilds = json.Value<JArray>("guilds").PopulateListJson<LoginGuild>();
                PrivateChannels = json.Value<JArray>("private_channels").PopulateListJson<PrivateChannel>();
                Presences = json.Value<JArray>("presences").PopulateListJson<DiscordPresence>();
            };
        }


        [JsonProperty("session_id")]
        internal string SessionId { get; private set; }


        [JsonProperty("user")]
        public DiscordClientUser User { get; private set; }


        [JsonProperty("guilds")]
        public IReadOnlyList<LoginGuild> Guilds { get; private set; }


        [JsonProperty("private_channels")]
        public List<PrivateChannel> PrivateChannels { get; private set; }


        [JsonProperty("relationships")]
        public List<Relationship> Relationships { get; private set; }


        [JsonProperty("user_guild_settings")]
        public IReadOnlyList<ClientGuildSettings> ClientGuildSettings { get; private set; }


        [JsonProperty("connected_accounts")]
        public IReadOnlyList<ClientConnectedAccount> ConnectedAccounts { get; private set; }


        [JsonProperty("user_settings")]
        public DiscordUserSettings Settings { get; private set; }


        [JsonProperty("presences")]
        public IReadOnlyList<DiscordPresence> Presences { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }


        public new void Dispose()
        {
            base.Dispose();
            SessionId = null;
            User.Dispose();
            User = null;
            foreach (var guild in Guilds)
                guild.Dispose();
            Guilds = null;
            foreach (var pc in PrivateChannels)
                pc.Dispose();
            PrivateChannels = null;
            Relationships = null;
            ClientGuildSettings = null;
            Settings = null;
        }
    }
}