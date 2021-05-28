using System;

namespace Discord
{
    [Flags]
    public enum DiscordPermission : ulong
    {
        None = 0,
        CreateInstantInvite = 0x0000000001,
        KickMembers = 0x0000000002,
        BanMembers = 0x0000000004,
        Administrator = 0x0000000008,
        ManageChannels = 0x0000000010,
        ManageGuild = 0x0000000020,
        AddReactions = 0x0000000040,
        ViewAuditLog = 0x0000000080,
        PrioritySpeaker = 0x0000000100,
        Stream = 0x0000000200,
        ViewChannel = 0x0000000400,
        SendMessages = 0x0000000800,
        SendTtsMessages = 0x0000001000,
        ManageMessages = 0x0000002000,
        EmbedLinks = 0x0000004000,
        AttachFiles = 0x0000008000,
        ReadMessageHistory = 0x0000010000,
        MentionEveryone = 0x0000020000,
        UseExternalEmojis = 0x0000040000,
        ViewGuildInsights = 0x0000080000,
        ConnectToVC = 0x0000100000,
        SpeakInVC = 0x0000200000,
        MuteMembers = 0x0000400000,
        DeafenVCMembers = 0x0000800000,
        MoveVCMembers = 0x0001000000,
        ForcePushToTalk = 0x0002000000,
        ChangeNickname = 0x0004000000,
        ManageNicknames = 0x0008000000,
        ManageRoles = 0x0010000000,
        ManageWebhook = 0x0020000000,
        ManageEmojis = 0x0040000000,
        SlashCommands = 0x0080000000,
        RequestToSpeakInStage = 0x0100000000,
        ManageThreads = 0x0400000000,
        PublicThreads = 0x0800000000,
        PrivateThreads = 0x1000000000
    }
}
