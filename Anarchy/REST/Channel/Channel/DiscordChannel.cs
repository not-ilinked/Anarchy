using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Discord
{
    public class DiscordChannel : MinimalChannel
    {
        [JsonProperty("name")]
        public string Name { get; private set; }


        [JsonProperty("type")]
        public ChannelType Type { get; private set; }


        protected void Update(DiscordChannel channel)
        {
            Json = channel.Json;
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


        public override string ToString()
        {
            return Name;
        }


        public static implicit operator ulong(DiscordChannel instance)
        {
            return instance.Id;
        }


        public GuildChannel ToGuildChannel()
        {
            if (Type == ChannelType.DM || Type == ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not of a guild");

            return Json.ToObject<GuildChannel>().SetJson(Json).SetClient(Client);
        }


        public TextChannel ToTextChannel()
        {
            if (Type != ChannelType.Text && Type != ChannelType.Store && Type != ChannelType.News)
                throw new InvalidConvertionException(Client, "Channel is not a guild text channel");

            return Json.ToObject<TextChannel>().SetJson(Json).SetClient(Client);
        }


        public VoiceChannel ToVoiceChannel()
        {
            if (Type == ChannelType.Text)
                throw new InvalidConvertionException(Client, "Channel is not a guild voice channel");

            return Json.ToObject<VoiceChannel>().SetJson(Json).SetClient(Client);
        }


        public PrivateChannel ToDMChannel()
        {
            if (Type != ChannelType.DM && Type != ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not a private channel");

            return Json.ToObject<PrivateChannel>().SetJson(Json).SetClient(Client);
        }


        public DiscordGroup ToGroup()
        {
            if (Type != ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not of type: Group");

            return Json.ToObject<DiscordGroup>().SetJson(Json).SetClient(Client);
        }
    }
}