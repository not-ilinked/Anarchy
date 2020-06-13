using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class GuildMemberProperties
    {
        private readonly Property<string> NickProperty = new Property<string>();
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


        private readonly Property<List<ulong>> RoleProperty = new Property<List<ulong>>();
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


        private readonly Property<bool> MuteProperty = new Property<bool>();
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


        private readonly Property<bool> DeafProperty = new Property<bool>();
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
