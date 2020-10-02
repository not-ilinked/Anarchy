using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public class DiscordHttpException : Exception
    {
        public DiscordError Code { get; private set; }
        public string ErrorMessage { get; private set; }
        /*
        public Dictionary<string, IReadOnlyList<InvalidParameter>> InvalidParameters { get; private set; }
        */
        internal DiscordHttpException(DiscordClient client, DiscordHttpError error) : base($"{(int)error.Code} {error.Message}")
        {
            Code = error.Code;
            ErrorMessage = error.Message;
            /*
            InvalidParameters = new Dictionary<string, IReadOnlyList<InvalidParameter>>();

            if (error.Fields != null)
            {
                foreach (var field in error.Fields)
                    InvalidParameters[field.Key] = field.Value.Fields;
            }*/
        }
    }
}