using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord.Gateway
{
    public class GuildSubscriptionProperties
    {
        [JsonProperty("guild_id")]
        internal ulong GuildId { get; set; }

        private readonly DiscordParameter<bool> _typeParam = new DiscordParameter<bool>();
        [JsonProperty("typing")]
        public bool Typing
        {
            get => _typeParam;
            set => _typeParam.Value = value;
        }

        public bool ShouldSerializeTyping()
        {
            return _typeParam.Set;
        }

        private readonly DiscordParameter<bool> _threadParam = new DiscordParameter<bool>();
        [JsonProperty("threads")]
        public bool Threads
        {
            get => _threadParam;
            set => _threadParam.Value = value;
        }

        public bool ShouldSerializeThreads()
        {
            return _threadParam.Set;
        }

        private readonly DiscordParameter<bool> _activityParam = new DiscordParameter<bool>();
        [JsonProperty("activities")]
        public bool Activities
        {
            get => _activityParam;
            set => _activityParam.Value = value;
        }

        public bool ShouldSerializeActivities()
        {
            return _activityParam.Set;
        }

        [JsonProperty("members")]
        public List<ulong> Members { get; set; } = new List<ulong>();

        [JsonProperty("channels")]
        public Dictionary<ulong, int[][]> Channels { get; set; } = new Dictionary<ulong, int[][]>();

        [JsonProperty("thread_member_lists")]
        private readonly List<object> _threadMemberLists = new List<object>();
    }
}
