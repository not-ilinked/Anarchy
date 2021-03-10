using System;

namespace Discord
{
    [Flags]
    public enum DiscordPermission : ulong
    {
        None = 0,
        CreateInstantInvite = 0x00000001,
        KickMembers = 0x00000002,
        BanMembers = 0x00000004,
        Administrator = 0x00000008,
        ManageChannels = 0x00000010,
        ManageGuild = 0x00000020,
        AddReactions = 0x00000040,
        ViewAuditLog = 0x00000080,
        PrioritySpeaker = 0x00000100,
        Stream = 0x00000200,
        ViewChannel = 0x00000400,
        SendMessages = 0x00000800,
        SendTtsMessages = 0x00001000,
        ManageMessages = 0x00002000,
        EmbedLinks = 0x00004000,
        AttachFiles = 0x00008000,
        ReadMessageHistory = 0x00010000,
        MentionEveryone = 0x00020000,
        UseExternalEmojis = 0x00040000,
        ViewGuildInsights = 0x00080000,
        ConnectToVC = 0x00100000,
        SpeakInVC = 0x00200000,
        MuteMembers = 0x00400000,
        DeafenVCMembers = 0x00800000,
        MoveVCMembers = 0x01000000,
        ForcePushToTalk = 0x02000000,
        ChangeNickname = 0x04000000,
        ManageNicknames = 0x08000000,
        ManageRoles = 0x10000000,
        ManageWebhook = 0x20000000,
        ManageEmojis = 0x40000000
    }
}
