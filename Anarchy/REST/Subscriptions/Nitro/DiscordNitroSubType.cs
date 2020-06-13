namespace Discord
{
    public class DiscordNitroSubType
    {
        public string Name { get; private set; }
        public ulong SkuId { get; private set; }
        public ulong SubscriptionPlanId { get; private set; }
        public int ExpectedAmount { get; private set; }

        public DiscordNitroSubType(string name, ulong skuId, ulong subPlanId, int expectedAmount)
        {
            Name = name;
            SkuId = skuId;
            SubscriptionPlanId = subPlanId;
            ExpectedAmount = expectedAmount;
        }

        public override string ToString()
        {
            string priceStr = ExpectedAmount.ToString();

            return $"{Name} {priceStr.Insert(priceStr.Length - 2, ".")}";
        }
    }
}
