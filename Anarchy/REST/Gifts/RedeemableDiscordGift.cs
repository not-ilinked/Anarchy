using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Discord
{
    public class RedeemableDiscordGift : DiscordGift
    {
        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; private set; }


        [JsonProperty("redeemed")]
        public bool Redeemed { get; private set; }


        [JsonProperty("uses")]
        public uint Uses { get; private set; }


        [JsonProperty("max_uses")]
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
