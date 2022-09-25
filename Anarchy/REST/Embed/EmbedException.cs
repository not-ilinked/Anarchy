using System;

namespace Discord
{
    public class EmbedException : Exception
    {
        public EmbedError Error { get; private set; }

        internal EmbedException(EmbedError error) : base(error.ToString())
        {
            Error = error;
        }
    }
}
