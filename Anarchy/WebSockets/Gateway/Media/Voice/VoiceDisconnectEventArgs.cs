using System;
using Discord.Media;
using Discord.WebSockets;

namespace Discord.Gateway
{
    public class VoiceDisconnectEventArgs : EventArgs
    {
        public MinimalGuild Guild { get; }

        public MinimalChannel Channel { get; }

        public DiscordMediaCloseCode Code { get; private set; }
        public string Reason { get; private set; }

        internal VoiceDisconnectEventArgs(DiscordSocketClient client, ulong? guildId, ulong channelId, DiscordWebSocketCloseEventArgs close)
        {
            if (guildId.HasValue) Guild = new MinimalGuild(guildId.Value).SetClient(client);
            Channel = new MinimalChannel(channelId).SetClient(client);

            Code = (DiscordMediaCloseCode) close.Code;
            Reason = close.Reason;
        }
    }
}
