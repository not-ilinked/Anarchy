namespace Discord.Media
{
    // format: guild:guild_id:channel_id:user_id
    public class StreamKey
    {
        public string Type { get; set; } = "guild";
        public ulong GuildId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }

        public static StreamKey Deserialize(string key)
        {
            string[] split = key.Split(':');

            return new StreamKey()
            {
                GuildId = ulong.Parse(split[1]),
                ChannelId = ulong.Parse(split[2]),
                UserId = ulong.Parse(split[3])
            };
        }

        public string Serialize()
        {
            return $"{Type}:{GuildId}:{ChannelId}:{UserId}";
        }
    }
}
