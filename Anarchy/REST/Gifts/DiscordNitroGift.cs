using Newtonsoft.Json;

namespace Discord
{
    public class DiscordNitroGift : DiscordGift
    {
        public ulong GifterId
        {
            get
            {
                return Gifter.Id;
            }
        }

        [JsonProperty("user")]
        public new DiscordUser Gifter { get; private set; }
    }
}
