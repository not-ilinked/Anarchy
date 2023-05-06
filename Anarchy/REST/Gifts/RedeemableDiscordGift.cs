using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discord
{
    public class RedeemableDiscordGift : DiscordGift
    {
        [JsonPropertyName("code")]
        public string Code { get; private set; }

        [JsonPropertyName("expires_at")]
        public DateTime ExpiresAt { get; private set; }

        [JsonPropertyName("redeemed")]
        public bool Redeemed { get; private set; }

        [JsonPropertyName("uses")]
        public uint Uses { get; private set; }

        [JsonPropertyName("max_uses")]
        public uint MaxUses { get; private set; }

        public async Task RedeemAsync(ulong? channelId = null)
        {
            await Client.RedeemGiftAsync(Code, channelId);
        }

        public void Redeem(ulong? channelId = null)
        {
            RedeemAsync(channelId).GetAwaiter().GetResult();
        }

        public async Task RevokeAsync()
        {
            await Client.RevokeGiftCodeAsync(Code);
        }

        public void Revoke()
        {
            RevokeAsync().GetAwaiter().GetResult();
        }
    }
}
