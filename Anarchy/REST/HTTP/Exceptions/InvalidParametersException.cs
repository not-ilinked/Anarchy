using System;
using System.Collections.Generic;

namespace Discord
{
    public class InvalidParametersException : Exception
    {
        public Dictionary<string, List<string>> Fields { get; private set; }

        public InvalidParametersException(Dictionary<string, List<string>> fields) : base(fields.Count + " invalid field(s)")
        {
            Fields = fields;
        }
    }
}
