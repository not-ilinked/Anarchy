using Newtonsoft.Json;

namespace Discord
{
    public class DiscordNitroGift : DiscordGift
    {
        public new ulong GifterId
        {
            get
            {
                return Gifter.Id;
            }
        }

        [JsonProperty("user")]
        public DiscordUser Gifter { get; private set; }
    }
}
