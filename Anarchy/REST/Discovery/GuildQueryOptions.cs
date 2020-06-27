namespace Discord
{
    public class GuildQueryOptions
    {
        public string Query { get; set; }
        public int Limit { get; set; } = 20;
        public int Offset { get; set; }
        public DiscoveryCategory? Category { get; set; }
    }
}
