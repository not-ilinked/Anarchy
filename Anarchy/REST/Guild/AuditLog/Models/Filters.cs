namespace Discord
{
    public class AuditLogFilters
    {
        internal DiscordParameter<long> UserIdProperty = new DiscordParameter<long>();
        public long UserId
        {
            get => UserIdProperty;
            set => UserIdProperty.Value = value;
        }


        internal DiscordParameter<AuditLogActionType> ActionTypeProperty = new DiscordParameter<AuditLogActionType>();
        public AuditLogActionType ActionType
        {
            get => ActionTypeProperty;
            set => ActionTypeProperty.Value = value;
        }


        internal DiscordParameter<ulong> BeforeIdProperty = new DiscordParameter<ulong>();
        public ulong BeforeId
        {
            get => BeforeIdProperty;
            set => BeforeIdProperty.Value = value;
        }


        internal DiscordParameter<uint> LimitProperty = new DiscordParameter<uint>();
        public uint Limit
        {
            get => LimitProperty;
            set => LimitProperty.Value = value;
        }
    }
}