namespace Discord
{
    public class SystemChannelInformation : Controllable
    {
        private readonly ulong? _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                if (_channelId.HasValue)
                    return new MinimalTextChannel(_channelId.Value).SetClient(Client);
                else
                    return null;
            }
        }

        private readonly int _flags;

        public bool SupressJoinNotifications
        {
            get { return (_flags & 1 << 0) == 1 << 0; }
        }

        public bool SupressBoostNotifications
        {
            get { return (_flags & 1 << 1) == 1 << 1; }
        }

        public SystemChannelInformation(ulong? channelId, int flags)
        {
            _channelId = channelId;
            _flags = flags;
        }
    }
}
