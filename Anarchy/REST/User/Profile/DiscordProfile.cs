using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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


        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("bio")]
        public string Biography { get; private set; }


        [JsonProperty("banner")]
        private readonly string _bannerHash;
        public DiscordCDNImage Banner => _bannerHash == null ? null : new DiscordCDNImage(CDNEndpoints.Banner, User.Id, _bannerHash);


        [JsonProperty("premium_since")]
        public DateTime? NitroSince { get; private set; }


        [JsonProperty("mutual_guilds")]
        public IReadOnlyList<MutualGuild> MutualGuilds { get; private set; }


        [JsonProperty("connected_accounts")]
        public IReadOnlyList<ConnectedAccount> ConnectedAccounts { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}
