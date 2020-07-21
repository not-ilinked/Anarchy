using System;

namespace Discord.Voice
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
