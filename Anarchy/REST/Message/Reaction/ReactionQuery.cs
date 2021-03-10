namespace Discord
{
    public class ReactionQuery
    {
        public string ReactionName { get; set; }
        public ulong? ReactionId { get; set; }

        public uint Limit { get; set; } = 25;
        public ulong AfterId { get; set; }
    }
}
