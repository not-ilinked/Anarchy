using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class DiscordHttpException : Exception
    {
        public DiscordError Code { get; private set; }
        public string ErrorMessage { get; private set; }

        public FieldErrorDictionary InvalidFields { get; private set; }

        public DiscordHttpException(DiscordHttpError error) : base($"{(int)error.Code} {error.Message}")
        {
            Code = error.Code;
            ErrorMessage = error.Message;

            if (error.Fields != null)
                InvalidFields = FindErrors(error.Fields);
        }

        private static FieldErrorDictionary FindErrors(JObject obj)
        {
            var dict = new FieldErrorDictionary();

            foreach (JProperty child in obj.Children())
            {
                if (child.Name == "_errors") dict.Errors = child.Value.ToObject<List<DiscordFieldError>>();
                else dict[child.Name] = FindErrors((JObject)child.Value);
            }

            return dict;
        }
    }
}