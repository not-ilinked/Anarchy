namespace Discord
{
    public class AuditLogFilters
    {
        internal Property<long> UserIdProperty = new Property<long>();
        public long UserId
        {
            get { return UserIdProperty; }
            set { UserIdProperty.Value = value; }
        }


        internal Property<AuditLogActionType> ActionTypeProperty = new Property<AuditLogActionType>();
        public AuditLogActionType ActionType
        {
            get { return ActionTypeProperty; }
            set { ActionTypeProperty.Value = value; }
        }


        internal Property<ulong> BeforeIdProperty = new Property<ulong>();
        public ulong BeforeId
        {
            get { return BeforeIdProperty; }
            set { BeforeIdProperty.Value = value; }
        }


        internal Property<uint> LimitProperty = new Property<uint>();
        public uint Limit
        {
            get { return LimitProperty; }
            set { LimitProperty.Value = value; }
        }
    }
}