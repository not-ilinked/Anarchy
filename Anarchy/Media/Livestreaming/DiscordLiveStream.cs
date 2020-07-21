using Discord.Gateway;
using Discord.Media;

namespace Discord.Streaming
{
    public class DiscordLiveStream
    {
        private ushort _sequence;
        private uint _timestamp;

        private readonly DiscordMediaSession _session;

        internal DiscordLiveStream(DiscordMediaSession session)
        {
            _session = session;
        }


        // WIP
        protected void Write(byte nalHeader, byte[] buffer, int offset, int count)
        {
            byte[] nalUnit = H264Packager.CreateNALUnit(nalHeader, buffer, offset, count);

            int length = new RTPPacketHeader() 
            { 
                // Version = 0x80, 
                Type = H264Packager.Codec.PayloadType, 
                Sequence = _sequence, 
                Timestamp = _timestamp, 
                SSRC = _session.SSRC.Video 
            }.Write(_session.SecretKey, nalUnit, 0, nalUnit.Length, out byte[] packet);

            _session.UdpClient.Send(packet, length);

            _sequence++;
            _timestamp++;
        }
        

        public void End()
        {
            if (_session.Server.StreamKey == null) // screenshare lol
                _session.Client.ChangeVoiceState(new VoiceStateChange() { GuildId = _session.Server.GuildId, ChannelId = _session.Channel.Id, Screensharing = false });
            else
                _session.Disconnect();
        }
    }
}
