namespace Discord
{
    public class MessageFilters
    {
        public MessageAttachmentFilter? Has { get; set; }


        public string Content { get; set; }


        public ulong? MentioningUserId { get; set; }


        public ulong? AuthorId { get; set; }


        public ulong? BeforeId { get; set; }


        public ulong? AfterId { get; set; }


        public uint? Limit { get; set; }
    }
}
