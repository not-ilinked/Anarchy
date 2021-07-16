using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class MemberListItem : Controllable
    {
        public MemberListItem()
        {
            OnClientUpdated += (s, e) => Member.SetClient(Client);
        }

        [JsonProperty("member")]
        public GuildMember Member { get; private set; }
    }
}
