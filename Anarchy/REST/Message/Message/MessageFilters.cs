namespace Discord
{
    public class MessageFilters
    {
        internal DiscordParameter<ulong> BeforeProperty = new DiscordParameter<ulong>();
        public ulong BeforeId
        {
            get { return BeforeProperty; }
            set { BeforeProperty.Value = value; }
        }


        internal DiscordParameter<ulong> AfterProperty = new DiscordParameter<ulong>();
        public ulong AfterId
        {
            get { return AfterProperty; }
            set { AfterProperty.Value = value; }
        }


        internal DiscordParameter<uint> LimitProperty = new DiscordParameter<uint>();
        public uint Limit
        {
            get { return LimitProperty; }
            set { LimitProperty.Value = value; }
        }


        internal DiscordParameter<ulong> UserProperty = new DiscordParameter<ulong>();
        public ulong UserId
        {
            get { return UserProperty; }
            set { UserProperty.Value = value; }
        }
    }
}
