using System.Collections.Generic;

namespace Discord
{
    public class FieldErrorDictionary : Dictionary<string, FieldErrorDictionary>
    {
        public IReadOnlyList<DiscordFieldError> Errors { get; set; }
    }
}
