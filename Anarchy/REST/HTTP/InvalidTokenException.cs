using System;

namespace Discord
{
    public class InvalidTokenException : Exception
    {
        public string Token { get; private set; }

        public InvalidTokenException(string token)
        {
            Token = token;
        }
    }
}
