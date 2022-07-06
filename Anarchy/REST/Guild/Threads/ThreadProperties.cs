using System;
using Newtonsoft.Json;

namespace Discord
{
    public class ThreadProperties
    {
        private readonly DiscordParameter<int> _durationParam = new DiscordParameter<int>();
        [JsonProperty("auto_archive_duration")]
        private int _duration => _durationParam.Value;

        public TimeSpan? TTL
        {
            get
            {
                if (_durationParam.Set) return new TimeSpan(0, _durationParam.Value, 0);
                return null;
            }

            set { _durationParam.Value = (int)value.Value.TotalMinutes; }
        }

        public bool ShouldSerialize_duration() => _durationParam.Set;


        private readonly DiscordParameter<bool> _archiveParam = new DiscordParameter<bool>();
        [JsonProperty("archived")]
        public bool Archived
        {
            get { return _archiveParam.Value; }
            set { _archiveParam.Value = value; }
        }

        public bool ShouldSerializeArchived() => _archiveParam.Set;


        private readonly DiscordParameter<string> _nameParam = new DiscordParameter<string>();
        [JsonProperty("name")]
        public string Name
        {
            get { return _nameParam.Value; }
            set { _nameParam.Value = value; }
        }

        public bool ShouldSerializeName() => _nameParam.Set;
    }
}
