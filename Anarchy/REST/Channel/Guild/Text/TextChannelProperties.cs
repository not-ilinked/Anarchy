﻿using Newtonsoft.Json;

namespace Discord
{
    /// <summary>
    /// Options for modifying a <see cref="TextChannel"/>
    /// </summary>
    public class TextChannelProperties : GuildChannelProperties
    {
        internal readonly DiscordParameter<ChannelType> TypeProperty = new DiscordParameter<ChannelType>();
        [JsonProperty("type")]
        private ChannelType _type => TypeProperty;

        public bool News
        {
            get => TypeProperty == ChannelType.News;
            set => TypeProperty.Value = value ? ChannelType.News : ChannelType.Text;
        }

        public bool ShouldSerialize_type()
        {
            return TypeProperty.Set;
        }

        private readonly DiscordParameter<string> TopicProperty = new DiscordParameter<string>();
        [JsonProperty("topic")]
        public string Topic
        {
            get => TopicProperty;
            set => TopicProperty.Value = value;
        }


        public bool ShouldSerializeTopic()
        {
            return TopicProperty.Set;
        }


        private readonly DiscordParameter<bool> NsfwProperty = new DiscordParameter<bool>();
        [JsonProperty("nsfw")]
        public bool Nsfw
        {
            get => NsfwProperty;
            set => NsfwProperty.Value = value;
        }


        public bool ShouldSerializeNsfw()
        {
            return NsfwProperty.Set;
        }


        private readonly DiscordParameter<int> SlowModeProperty = new DiscordParameter<int>();
        [JsonProperty("rate_limit_per_user")]
        public int SlowMode
        {
            get => SlowModeProperty;
            set => SlowModeProperty.Value = value;
        }


        public bool ShouldSerializeSlowMode()
        {
            return SlowModeProperty.Set;
        }
    }
}
