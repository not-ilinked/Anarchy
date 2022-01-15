namespace Discord.Media
{
    internal class StreamKey
    {
        internal StreamKey(string key)
        {
            string[] parts = key.Split(':');

            Location = parts[0];
            GuildId = ulong.Parse(parts[1]);
            ChannelId = ulong.Parse(parts[2]);
            UserId = ulong.Parse(parts[3]);
        }

        internal StreamKey(ulong guildId, ulong channelId, ulong userId)
        {
            Location = "guild";
            GuildId = guildId;
            ChannelId = channelId;
            UserId = userId;
        }

        public string Location { get; }
        public ulong GuildId { get; }
        public ulong ChannelId { get; }
        public ulong UserId { get; }

        public string Serialize()
        {
            return $"{Location}:{GuildId}:{ChannelId}:{UserId}";
        }
    }
}
