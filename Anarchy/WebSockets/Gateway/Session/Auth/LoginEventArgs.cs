using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class LoginEventArgs : Controllable, IDisposable
    {
        internal LoginEventArgs()
        {
            OnClientUpdated += (sender, e) =>
            {
                User.SetClient(Client);
                Relationships.SetClientsInList(Client);
                Settings.SetClient(Client);
                PrivateChannels.SetClientsInList(Client);
                ClientGuildSettings.SetClientsInList(Client);
                ConnectedAccounts.SetClientsInList(Client);
                Presences.SetClientsInList(Client);

                List<MinimalGuild> guilds = new List<MinimalGuild>();
                foreach (var obj in _guilds)
                    guilds.Add((Client.User.Type == DiscordUserType.User ? obj.ToObject<SocketGuild>() : obj.ToObject<MinimalGuild>()).SetClient(Client));
                Guilds = guilds;
            };
        }


        [JsonProperty("session_id")]
        internal string SessionId { get; private set; }


        [JsonProperty("user")]
        public DiscordClientUser User { get; private set; }


        [JsonProperty("status")]
        public UserStatus Status { get; private set; }


        [JsonProperty("locale")]
        public DiscordLanguage Language { get; private set; }


        [JsonProperty("user_settings")]
        public DiscordUserSettings Settings { get; private set; }


        [JsonProperty("guilds")]
        private readonly List<JObject> _guilds;

        public IReadOnlyList<MinimalGuild> Guilds { get; private set; }


        [JsonProperty("private_channels")]
        [JsonConverter(typeof(DeepJsonConverter<PrivateChannel>))]
        private readonly List<PrivateChannel> _channels;

        public IReadOnlyList<PrivateChannel> PrivateChannels
        {
            get { return _channels; }
        }


        [JsonProperty("relationships")]
        public IReadOnlyList<DiscordRelationship> Relationships { get; private set; }


        [JsonProperty("user_guild_settings")]
        internal IReadOnlyList<ClientGuildSettings> ClientGuildSettings { get; private set; }


        [JsonProperty("connected_accounts")]
        public IReadOnlyList<ClientConnectedAccount> ConnectedAccounts { get; private set; }


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
            _channels.Clear();
            Relationships = null;
            ClientGuildSettings = null;
            Settings = null;
        }
    }
}