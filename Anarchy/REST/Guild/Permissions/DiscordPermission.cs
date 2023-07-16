using System;

namespace Discord
{
    [Flags]
    public enum DiscordPermission : ulong
    {
        None = 0,
        CreateInstantInvite = 0x0000000001, // 1 << 0
        KickMembers = 0x0000000002, // 1 << 1
        BanMembers = 0x0000000004, // 1 << 2
        Administrator = 0x0000000008, // 1 << 3
        ManageChannels = 0x0000000010, // 1 << 4
        ManageGuild = 0x0000000020, // 1 << 5
        AddReactions = 0x0000000040, // 1 << 6
        ViewAuditLog = 0x0000000080, // 1 << 7
        PrioritySpeaker = 0x0000000100, // 1 << 8
        Stream = 0x0000000200, // 1 << 9
        ViewChannel = 0x0000000400, // 1 << 10
        SendMessages = 0x0000000800, // 1 << 11
        SendTtsMessages = 0x0000001000, // 1 << 12
        ManageMessages = 0x0000002000, // 1 << 13
        EmbedLinks = 0x0000004000, // 1 << 14
        AttachFiles = 0x0000008000, // 1 << 15
        ReadMessageHistory = 0x0000010000, // 1 << 16
        MentionEveryone = 0x0000020000, // 1 << 17
        UseExternalEmojis = 0x0000040000, // 1 << 18
        ViewGuildInsights = 0x0000080000, // 1 << 19
        ConnectToVC = 0x0000100000, // 1 << 20
        SpeakInVC = 0x0000200000, // 1 << 21
        MuteMembers = 0x0000400000, // 1 << 22
        DeafenVCMembers = 0x0000800000, // 1 << 23
        MoveVCMembers = 0x0001000000, // 1 << 24
        ForcePushToTalk = 0x0002000000, // 1 << 25
        ChangeNickname = 0x0004000000, // 1 << 26
        ManageNicknames = 0x0008000000, // 1 << 27
        ManageRoles = 0x0010000000, // 1 << 28
        ManageWebhook = 0x0020000000, // 1 << 29
        ManageEmojis = 0x0040000000, // 1 << 30
        SlashCommands = 0x0080000000, // 1 << 31
        RequestToSpeakInStage = 0x0100000000, // 1 << 32
        ManageEvents = 0x0200000000, // 1 << 33
        ManageThreads = 0x0400000000, // 1 << 34
        PublicThreads = 0x0800000000, // 1 << 35
        PrivateThreads = 0x1000000000, // 1 << 36
        UseExternalStickers = 0x2000000000, // 1 << 37
        SendMessagesInThreads = 0x4000000000, // 1 << 38
        UseEmbeddedActivities = 0x8000000000, // 1 << 39
        ModerateMembers = 0x10000000000, // 1 << 40
        ViewCreatorMonetizationAnalytics = 0x20000000000, // 1 << 41
        UseSoundboard = 0x40000000000, // 1 << 42
        UseExternalSounds = 0x200000000000, // 1 << 45
        SendVoiceMessages = 0x400000000000, // 1 << 46
    }
}
