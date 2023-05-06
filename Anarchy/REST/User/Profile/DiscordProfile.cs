using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordProfile : Controllable
    {
        public DiscordProfile()
        {
            OnClientUpdated += (sender, e) =>
            {
                User.SetClient(Client);
                MutualGuilds.SetClientsInList(Client);
                ConnectedAccounts.SetClientsInList(Client);
            };
        }

        /// <summary>
        /// Updates the profile's info
        /// </summary>
        public void Update()
        {
            DiscordProfile profile = Client.GetProfile(User.Id);
            User = profile.User;
            MutualGuilds = profile.MutualGuilds;
            ConnectedAccounts = profile.ConnectedAccounts;
        }

        [JsonPropertyName("user")]
        public DiscordProfileUser User { get; private set; }

        [JsonPropertyName("premium_since")]
        public DateTime? NitroSince { get; private set; }

        [JsonPropertyName("mutual_guilds")]
        public IReadOnlyList<MutualGuild> MutualGuilds { get; private set; }

        [JsonPropertyName("connected_accounts")]
        public IReadOnlyList<ConnectedAccount> ConnectedAccounts { get; private set; }

        public override string ToString()
        {
            return User.ToString();
        }
    }
}
