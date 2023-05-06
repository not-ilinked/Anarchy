using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Discord
{
    public class DiscordHttpException : Exception
    {
        public DiscordError Code { get; private set; }
        public string ErrorMessage { get; private set; }

        public FieldErrorDictionary InvalidFields { get; private set; }

        public DiscordHttpException(DiscordHttpError error) : base($"{(int) error.Code} {error.Message}")
        {
            Code = error.Code;
            ErrorMessage = error.Message;

            if (error.Fields != null)
                InvalidFields = FindErrors(error.Fields);
        }

        private static FieldErrorDictionary FindErrors(JsonElement element)
        {
            var dict = new FieldErrorDictionary();

            if (element.ValueKind != JsonValueKind.Object)
            {
                return dict;
            }

            foreach (JsonProperty child in element.EnumerateObject())
            {
                if (child.Name == "_errors")
                {
                    dict.Errors = JsonSerializer.Deserialize<List<DiscordFieldError>>(child.Value.GetRawText());
                }
                else
                {
                    dict[child.Name] = FindErrors(child.Value);
                }
            }

            return dict;
        }
    }
}