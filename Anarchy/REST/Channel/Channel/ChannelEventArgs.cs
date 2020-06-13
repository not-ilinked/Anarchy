namespace Discord
{
    public class ChannelEventArgs
    {
        public DiscordChannel Channel { get; private set; }

        public ChannelEventArgs(DiscordChannel channel)
        {
            Channel = channel;
        }


        public override string ToString()
        {
            return Channel.ToString();
        }
    }
}