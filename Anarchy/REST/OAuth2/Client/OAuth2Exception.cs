using System;

namespace Discord
{
    public class OAuth2Exception : Exception
    {
        public string Error { get; }

        internal OAuth2Exception(OAuth2HttpError err) : base(err.Description)
        {
            Error = err.Error;
        }
    }
}
