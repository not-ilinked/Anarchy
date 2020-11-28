namespace Discord
{
    public class AuditLogFilters
    {
        internal DiscordParameter<long> UserIdProperty = new DiscordParameter<long>();
        public long UserId
        {
            get { return UserIdProperty; }
            set { UserIdProperty.Value = value; }
        }


        internal DiscordParameter<AuditLogActionType> ActionTypeProperty = new DiscordParameter<AuditLogActionType>();
        public AuditLogActionType ActionType
        {
            get { return ActionTypeProperty; }
            set { ActionTypeProperty.Value = value; }
        }


        internal DiscordParameter<ulong> BeforeIdProperty = new DiscordParameter<ulong>();
        public ulong BeforeId
        {
            get { return BeforeIdProperty; }
            set { BeforeIdProperty.Value = value; }
        }


        internal DiscordParameter<uint> LimitProperty = new DiscordParameter<uint>();
        public uint Limit
        {
            get { return LimitProperty; }
            set { LimitProperty.Value = value; }
        }
    }
}