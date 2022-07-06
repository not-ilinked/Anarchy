using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord
{
    public class GuildMemberProperties
    {


        internal readonly DiscordParameter<string> NickProperty = new DiscordParameter<string>();
        [JsonProperty("nick")]
        public string Nickname
        {
            get { return NickProperty; }
            set { NickProperty.Value = value; }
        }


        public bool ShouldSerializeNickname()
        {
            return NickProperty.Set;
        }


        internal readonly DiscordParameter<List<ulong>> RoleProperty = new DiscordParameter<List<ulong>>();
        [JsonProperty("roles")]
        public List<ulong> Roles
        {
            get { return RoleProperty; }
            set { RoleProperty.Value = value; }
        }


        public bool ShouldSerializeRoles()
        {
            return RoleProperty.Set;
        }


        private readonly DiscordParameter<ulong> ChannelProperty = new DiscordParameter<ulong>();
        [JsonProperty("channel_id")]
        public ulong ChannelId
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }


        private readonly DiscordParameter<bool> MuteProperty = new DiscordParameter<bool>();
        [JsonProperty("mute")]
        public bool Muted
        {
            get { return MuteProperty; }
            set { MuteProperty.Value = value; }
        }


        public bool ShouldSerializeMuted()
        {
            return MuteProperty.Set;
        }


        private readonly DiscordParameter<bool> DeafProperty = new DiscordParameter<bool>();
        [JsonProperty("deaf")]
        public bool Deafened
        {
            get { return DeafProperty; }
            set { DeafProperty.Value = value; }
        }


        public bool ShouldSerializeDeafened()
        {
            return DeafProperty.Set;
        }
    }
}
