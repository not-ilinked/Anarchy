using System;

namespace Discord.Media
{
    internal class OpusException : Exception
    {
        public OpusError Error { get; private set; }

        public OpusException(OpusError error) : base(error.ToString())
        {
            Error = error;
        }
    }
}
