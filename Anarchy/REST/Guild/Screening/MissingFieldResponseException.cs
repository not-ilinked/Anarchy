using System;

namespace Discord
{
    public class MissingFieldResponseException : Exception
    {
        public MissingFieldResponseException(string label) : base($"The field \"{label}\" is required and therefore .Response must be set")
        { }
    }
}
