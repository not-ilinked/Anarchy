using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class Activity
    {
        public Activity()
        { }


        public Activity(string name, ActivityType type) : this()
        {
            Name = name;
            Type = type;
        }


        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("type")]
        public ActivityType Type { get; set; }


        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
