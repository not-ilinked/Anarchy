

using System.Text.Json.Serialization;

namespace Discord
{
    public class MutualGuild : MinimalGuild
    {
        [JsonPropertyName("nick")]
        public string Nickname { get; private set; }

        public override string ToString()
        {
            return Id.ToString();
        }

        public static implicit operator ulong(MutualGuild instance)
        {
            return instance.Id;
        }
    }
}
