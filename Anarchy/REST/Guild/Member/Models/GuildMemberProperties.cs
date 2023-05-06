using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord
{
    public class GuildMemberProperties
    {

        internal readonly DiscordParameter<string> NickProperty = new DiscordParameter<string>();
        [JsonPropertyName("nick")]
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
        [JsonPropertyName("roles")]
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
        [JsonPropertyName("channel_id")]
        public ulong ChannelId
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }

        private readonly DiscordParameter<bool> MuteProperty = new DiscordParameter<bool>();
        [JsonPropertyName("mute")]
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
        [JsonPropertyName("deaf")]
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
