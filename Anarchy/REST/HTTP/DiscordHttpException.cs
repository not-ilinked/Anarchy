namespace Discord
{
    public class DiscordHttpException : DiscordException
    {
        public DiscordError Code { get; private set; }
        public string ErrorMessage { get; private set; }

        internal DiscordHttpException(DiscordClient client, DiscordHttpError error) : base(client, $"{(int)error.Code} {error.Message}")
        {
            Code = error.Code;
            ErrorMessage = error.Message;
        }
    }
}