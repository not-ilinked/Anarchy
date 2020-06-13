namespace Discord
{
    public class MessageFilters
    {
        internal Property<ulong> BeforeProperty = new Property<ulong>();
        public ulong BeforeId
        {
            get { return BeforeProperty; }
            set { BeforeProperty.Value = value; }
        }


        internal Property<ulong> AfterProperty = new Property<ulong>();
        public ulong AfterId
        {
            get { return AfterProperty; }
            set { AfterProperty.Value = value; }
        }


        internal Property<uint> LimitProperty = new Property<uint>();
        public uint Limit
        {
            get { return LimitProperty; }
            set { LimitProperty.Value = value; }
        }


        internal Property<ulong> UserProperty = new Property<ulong>();
        public ulong UserId
        {
            get { return UserProperty; }
            set { UserProperty.Value = value; }
        }
    }
}
