using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("session_id")]
        internal string SessionId { get; private set; }

        [JsonPropertyName("user")]
        public DiscordClientUser User { get; private set; }

        [JsonPropertyName("status")]
        public UserStatus Status { get; private set; }

        [JsonPropertyName("locale")]
        public DiscordLanguage Language { get; private set; }

        [JsonPropertyName("user_settings")]
        public DiscordUserSettings Settings { get; private set; }

        [JsonPropertyName("guilds")]
        private readonly List<JObject> _guilds;

        public IReadOnlyList<MinimalGuild> Guilds { get; private set; }

        [JsonPropertyName("private_channels")]
        [JsonConverter(typeof(DeepJsonConverter<PrivateChannel>))]
        private readonly List<PrivateChannel> _channels;

        public IReadOnlyList<PrivateChannel> PrivateChannels
        {
            get { return _channels; }
        }

        [JsonPropertyName("relationships")]
        public IReadOnlyList<DiscordRelationship> Relationships { get; private set; }

        [JsonPropertyName("user_guild_settings")]
        internal IReadOnlyList<ClientGuildSettings> ClientGuildSettings { get; private set; }

        [JsonPropertyName("connected_accounts")]
        public IReadOnlyList<ClientConnectedAccount> ConnectedAccounts { get; private set; }

        [JsonPropertyName("presences")]
        public IReadOnlyList<DiscordPresence> Presences { get; private set; }

        [JsonPropertyName("application")]
        internal JsonElement Application { get; private set; }

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