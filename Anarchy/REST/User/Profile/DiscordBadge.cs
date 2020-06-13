using System;

namespace Discord
{
    [Flags]
    public enum DiscordBadge
    {
        None,
        DiscordEmployeee = 1 << 0,
        DiscordPartner = 1 << 1,
        HypesquadEvents = 1 << 2,
        BugHunter = 1 << 3,
        LocalUser = 1 << 5,
        HypeBravery = 1 << 6,
        HypeBrilliance = 1 << 7,
        HypeBalance = 1 << 8,
        EarlySupporter = 1 << 9,
        TeamUser = 1 << 10,
        System = 1 << 2,
        BugHunterLevel2 = 1 << 14,
        VerifiedBot = 1 << 16,
        VerifiedBotDeveloper = 1 << 17
    }
}
