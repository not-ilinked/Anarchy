

using System.Text.Json.Serialization;

namespace Discord
{
    public class DiscordBan : Controllable
    {
        public DiscordBan()
        {
            OnClientUpdated += (sender, e) => User.SetClient(Client);
        }

        [JsonPropertyName("reason")]
        public string Reason { get; private set; }

        [JsonPropertyName("user")]
        public DiscordUser User { get; private set; }

        internal ulong GuildId { get; set; }

        public MinimalGuild Guild
        {
            get
            {
                return new MinimalGuild(GuildId).SetClient(Client);
            }
        }

        /// <summary>
        /// Unbans the user
        /// </summary>
        public void Unban()
        {
            Client.UnbanGuildMember(GuildId, User.Id);
        }

        public override string ToString()
        {
            return User.ToString();
        }
    }
}
