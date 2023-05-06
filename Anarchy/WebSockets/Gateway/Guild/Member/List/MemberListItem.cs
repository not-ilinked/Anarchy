

using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class MemberListItem : Controllable
    {
        public MemberListItem()
        {
            OnClientUpdated += (s, e) => Member.SetClient(Client);
        }

        [JsonPropertyName("member")]
        public GuildMember Member { get; private set; }
    }
}
