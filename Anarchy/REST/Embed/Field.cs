using Newtonsoft.Json;

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

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("value")]
        public string Content { get; private set; }

        [JsonProperty("inline")]
        public bool Inline { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
