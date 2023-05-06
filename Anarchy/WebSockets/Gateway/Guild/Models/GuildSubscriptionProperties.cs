using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Discord.Gateway
{
    public class GuildSubscriptionProperties
    {
        [JsonPropertyName("guild_id")]
        internal ulong GuildId { get; set; }

        private readonly DiscordParameter<bool> _typeParam = new DiscordParameter<bool>();
        [JsonPropertyName("typing")]
        public bool Typing
        {
            get { return _typeParam; }
            set { _typeParam.Value = value; }
        }

        public bool ShouldSerializeTyping() => _typeParam.Set;

        private readonly DiscordParameter<bool> _threadParam = new DiscordParameter<bool>();
        [JsonPropertyName("threads")]
        public bool Threads
        {
            get { return _threadParam; }
            set { _threadParam.Value = value; }
        }

        public bool ShouldSerializeThreads() => _threadParam.Set;

        private readonly DiscordParameter<bool> _activityParam = new DiscordParameter<bool>();
        [JsonPropertyName("activities")]
        public bool Activities
        {
            get { return _activityParam; }
            set { _activityParam.Value = value; }
        }

        public bool ShouldSerializeActivities() => _activityParam.Set;

        [JsonPropertyName("members")]
        public List<ulong> Members { get; set; } = new List<ulong>();

        [JsonPropertyName("channels")]
        public Dictionary<ulong, int[][]> Channels { get; set; } = new Dictionary<ulong, int[][]>();

        [JsonPropertyName("thread_member_lists")]
        private readonly List<object> _threadMemberLists = new List<object>();
    }
}
