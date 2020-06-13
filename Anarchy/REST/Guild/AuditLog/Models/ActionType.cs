namespace Discord
{
    public enum AuditLogActionType
    {
        GuildUpdate = 1,
        ChannelCreate = 10,
        ChannelUpdate,
        ChannelDelete,
        ChannelOverwriteCreate,
        ChannelOverwriteUpdate,
        ChannelOverwriteDelete,
        MemberKick = 20,
        MemberPrune,
        MemberBan,
        MemberUnban,
        MemberUpdate,
        MemberRoleUpdate,
        RoleCreate = 30,
        RoleUpdate,
        RoleDelete,
        InviteCreate = 40,
        InviteUpdate,
        InviteDlete,
        WebhookCreate = 50,
        WebhookUpdate,
        WebhookDelete,
        ReactionCreate = 60,
        ReactionUpdate,
        ReactionDelete,
        MessageDelete = 72
    }
}
