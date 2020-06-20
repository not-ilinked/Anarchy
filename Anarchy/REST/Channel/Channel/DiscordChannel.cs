using Newtonsoft.Json;

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


        /// <summary>
        /// Updates the channel's info
        /// </summary>
        public void Update()
        {
            Update(Client.GetChannel(Id));
        }


        /// <summary>
        /// Modifies the channel
        /// </summary>
        /// <param name="properties">Options for modifying the channel</param>
        public new void Modify(ChannelProperties properties)
        {
            Update(base.Modify(properties));
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


        public Group ToGroup()
        {
            if (Type != ChannelType.Group)
                throw new InvalidConvertionException(Client, "Channel is not of type: Group");

            return Json.ToObject<Group>().SetJson(Json).SetClient(Client);
        }
    }
}