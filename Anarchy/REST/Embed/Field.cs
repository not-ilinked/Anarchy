using System.Text.Json.Serialization;

namespace Discord
{
    public class EmbedField
    {
        public EmbedField()
        {

        }

        internal EmbedField(string name, string content, bool inline)
        {
            Name = name;
            Content = content;
            Inline = inline;
        }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("value")]
        public string Content { get; private set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
