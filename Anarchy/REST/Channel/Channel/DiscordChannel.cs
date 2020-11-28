using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordChannel : MinimalChannel
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("type")]
        public ChannelType Type { get; protected set; }


        public bool InGuild
        {
            get { return Type != ChannelType.DM && Type != ChannelType.Group; }
        }

        public bool IsText
        {
            get { return Type != ChannelType.Category && Type != ChannelType.Voice; }
        }

        public bool IsVoice
        {
            get { return Type == ChannelType.Voice || Type == ChannelType.DM || Type == ChannelType.Group; }
        }


        protected void Update(DiscordChannel channel)
        {
            Name = channel.Name;
        }

        public async Task UpdateAsync()
        {
            Update(await Client.GetChannelAsync(Id));
        }

        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public void Update()
        {
            UpdateAsync().GetAwaiter().GetResult();
        }


        // retarded solution but it's the best i could come up with
        internal virtual void SetLastMessageId(ulong id) { }


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(DiscordChannel instance)
        {
            return instance.Id;
        }
    }
}