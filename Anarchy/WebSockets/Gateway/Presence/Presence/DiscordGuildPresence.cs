using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class DiscordGuildPresence : DiscordPresence
    {
        [JsonProperty("roles")]
        public IReadOnlyList<ulong> Roles { get; private set; }


        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(_guildId.Value).SetClient(Client);
            }
        }


        [JsonProperty("premium_since")]
        public DateTime? BoostingSince { get; private set; }


        [JsonProperty("nick")]
        public string Nickname { get; private set; }
    }
}
