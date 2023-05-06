using System.Text.Json.Serialization;

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

        [JsonPropertyName("user")]
        public new DiscordUser Gifter { get; private set; }
    }
}
