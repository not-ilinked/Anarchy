using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public class ResolvedInteractionData : Controllable
    {
        public ResolvedInteractionData()
        {
            OnClientUpdated += (s, e) =>
            {
                if (Channels != null)
                {
                    Channels.Values.ToList().SetClientsInList(Client);
                }

                if (Users != null)
                {
                    Users.Values.ToList().SetClientsInList(Client);

                    if (Members != null)
                    {
                        Members.Values.ToList().SetClientsInList(Client);

                        foreach (DiscordUser user in Users.Values)
                        {
                            Members[user.Id].User = user;
                        }
                    }
                }

                if (Roles != null)
                {
                    Roles.Values.ToList().SetClientsInList(Client);
                }
            };
        }

        [JsonProperty("channels")]
        public Dictionary<ulong, DiscordChannel> Channels { get; private set; }

        [JsonProperty("users")]
        public Dictionary<ulong, DiscordUser> Users { get; private set; }

        [JsonProperty("members")]
        public Dictionary<ulong, GuildMember> Members { get; private set; }

        [JsonProperty("roles")]
        public Dictionary<ulong, DiscordRole> Roles { get; private set; }
    }
}
