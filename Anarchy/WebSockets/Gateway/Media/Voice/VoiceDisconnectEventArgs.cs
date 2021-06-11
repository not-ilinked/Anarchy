using Discord.Media;
using System;

namespace Discord.Gateway
{
    public class VoiceDisconnectEventArgs : EventArgs
    {
        private readonly ulong? _guildId;
        public MinimalGuild Guild => _guildId.HasValue ? new MinimalGuild(_guildId.Value) : null;

        private readonly ulong? _channelId;
        public MinimalChannel Channel => _channelId.HasValue ? new MinimalChannel(_channelId.Value) : null;

        public DiscordMediaCloseCode Code { get; private set; }
        public string Reason { get; private set; }

        internal VoiceDisconnectEventArgs(ulong? guildId, ulong channelId, WebSocketSharp.CloseEventArgs close)
        {
            _guildId = guildId;
            _channelId = channelId;

            Code = (DiscordMediaCloseCode)close.Code;
            Reason = close.Reason;
        }
    }
}
