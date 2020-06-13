namespace Discord
{
    public class InvalidConvertionException : DiscordException
    {
        internal InvalidConvertionException(DiscordClient client, string message) : base(client, message)
        {
        }
    }
}
