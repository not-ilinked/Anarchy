using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord
{
    public class SystemChannelInformation
    {
        private readonly ulong? _channelId;

        public MinimalTextChannel Channel
        {
            get
            {
                if (_channelId.HasValue)
                    return new MinimalTextChannel(_channelId.Value);
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
