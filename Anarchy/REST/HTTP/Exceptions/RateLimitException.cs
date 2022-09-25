using System;

namespace Discord
{
    public class RateLimitException : Exception
    {
        public int RetryAfter { get; private set; }

        public RateLimitException(int retryAfter) : base($"Ratelimited for {retryAfter} milliseconds")
        {
            RetryAfter = retryAfter;
        }

        public override string ToString()
        {
            return RetryAfter.ToString();
        }
    }
}