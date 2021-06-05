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
        
        public Dictionary<string, Dictionary<string, string>> InvalidParameters { get; private set; }
        
        public DiscordHttpException(DiscordHttpError error) : base($"{(int)error.Code} {error.Message}")
        {
            Code = error.Code;
            ErrorMessage = error.Message;
            
            InvalidParameters = new Dictionary<string, Dictionary<string, string>>();

            if (error.Fields != null)
            {
                foreach (var field in error.Fields)
                {
                    var errors = InvalidParameters[field.Key] = new Dictionary<string, string>();

                    foreach (var child in field.Value.Children())
                    {
                        try
                        {
                            var param = FindError((JObject)((JProperty)child).Value);
                            errors[param.Code] = param.Message;
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        private InvalidParameter FindError(JObject obj)
        {
            if (obj.TryGetValue("_errors", out var value)) return value.Children().ElementAt(0).ToObject<InvalidParameter>();
            else return FindError((JObject)((JProperty)obj.Children().ElementAt(0)).Value);
        }
    }
}