namespace Discord
{
    public class RateLimitException : DiscordException
    {
        public int RetryAfter { get; private set; }

        internal RateLimitException(DiscordClient client, int retryAfter) : base(client, $"Ratelimited for {retryAfter} milliseconds")
        {
            RetryAfter = retryAfter;
        }


        public override string ToString()
        {
            return RetryAfter.ToString();
        }
    }
}