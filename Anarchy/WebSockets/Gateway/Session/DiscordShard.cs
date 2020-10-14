namespace Discord
{
    public class DiscordShard
    {
        public uint Index { get; set; }
        public uint Total { get; set; }

        public DiscordShard(uint index, uint total)
        {
            Index = index;
            Total = total;
        }
    }
}
