using System.Collections.Generic;
using System.Linq;

namespace Discord.Gateway
{
    public class DiscordSessionsEventArgs
    {
        public IReadOnlyList<DiscordSession> Sessions { get; private set; }

        internal DiscordSessionsEventArgs(List<DiscordSession> sessions)
        {
            Sessions = sessions;
        }
    }
}
