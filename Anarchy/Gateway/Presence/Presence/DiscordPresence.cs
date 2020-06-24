using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class DiscordPresence : Controllable
    {
        [JsonProperty("user")]
        public DiscordUser User { get; private set; }


        [JsonProperty("nick")]
        public string Nickname { get; private set; }


        [JsonProperty("roles")]
        public IReadOnlyList<ulong> Roles { get; private set; }


        [JsonProperty("game")]
        public UserActivity Activity { get; private set; }


        [JsonProperty("activities")]
        public IReadOnlyList<UserActivity> Activities { get; private set; }


        [JsonProperty("guild_id")]
        private ulong? _guildId;

        public MinimalGuild Guild
        {
            get
            {
                if (_guildId.HasValue)
                    return new MinimalGuild(_guildId.Value).SetClient(Client);
                else
                    return null;
            }
        }


        [JsonProperty("status")]
        private string _status;
        public UserStatus Status
        {
            get
            {
                if (_status == "dnd")
                    return UserStatus.DoNotDisturb;
                else
                    return (UserStatus)Enum.Parse(typeof(UserStatus), _status, true);
            }
            set
            {
                _status = value != UserStatus.DoNotDisturb ? value.ToString().ToLower() : "dnd";
            }
        }


        [JsonProperty("premium_since")]
        public DateTime? BoostingSince { get; private set; }


        public override string ToString()
        {
            return User.ToString();
        }
    }
}
