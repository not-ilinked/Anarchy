using Newtonsoft.Json;
using System.Collections.Generic;

namespace Discord
{
    public class RowComponent : MessageComponent
    {
        public RowComponent() 
        {
            Type = MessageComponentType.Row;
        }

        public RowComponent(List<MessageComponent> children) : this()
        {
            Components = children;
        }

        [JsonProperty("components")]
        [JsonConverter(typeof(DeepJsonConverter<MessageComponent>))]
        public List<MessageComponent> Components { get; private set; }
    }
}
