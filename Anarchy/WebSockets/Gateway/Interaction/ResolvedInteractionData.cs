using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class ResolvedInteractionData : Controllable
    {
        public ResolvedInteractionData()
        {
            OnClientUpdated += (s, e) =>
            {
                if (Channels != null)
                    Channels.Values.ToList().SetClientsInList(Client);

                if (Users != null)
                {
                    Users.Values.ToList().SetClientsInList(Client);

                    if (Members != null)
                    {
                        Members.Values.ToList().SetClientsInList(Client);

                        foreach (var user in Users.Values)
                            Members[user.Id].User = user;
                    }
                }

                if (Roles != null) Roles.Values.ToList().SetClientsInList(Client);
            };
        }

        [JsonPropertyName("channels")]
        public Dictionary<ulong, DiscordChannel> Channels { get; private set; }

        [JsonPropertyName("users")]
        public Dictionary<ulong, DiscordUser> Users { get; private set; }

        [JsonPropertyName("members")]
        public Dictionary<ulong, GuildMember> Members { get; private set; }

        [JsonPropertyName("roles")]
        public Dictionary<ulong, DiscordRole> Roles { get; private set; }
    }
}
