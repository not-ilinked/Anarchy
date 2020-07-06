using Discord.Commands;
using Discord.Voice;
using Leaf.xNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Reflection;
using System.IO;

namespace Discord.Gateway
{
    internal class VoiceSessionInfo
    {
        public VoiceSessionInfo(DiscordVoiceSession session, ulong id)
        {
            Session = session;
            Id = id;
        }

        public DiscordVoiceSession Session { get; set; }
        public ulong Id { get; set; }
    }

    /// <summary>
    /// <see cref="DiscordClient"/> with Gateway support
    /// </summary>
    public class DiscordSocketClient : DiscordClient, IDisposable
    {
        #region events
        public delegate void UserHandler(DiscordSocketClient client, UserEventArgs args);
        public delegate void ChannelHandler(DiscordSocketClient client, ChannelEventArgs args);
        public delegate void VoiceStateHandler(DiscordSocketClient client, VoiceStateEventArgs args);
        public delegate void MessageHandler(DiscordSocketClient client, MessageEventArgs args);
        public delegate void ReactionHandler(DiscordSocketClient client, ReactionEventArgs args);
        public delegate void RoleHandler(DiscordSocketClient client, RoleEventArgs args);
        public delegate void BanUpdateHandler(DiscordSocketClient client, BanUpdateEventArgs args);
        public delegate void RelationshipHandler(DiscordSocketClient client, RelationshipEventArgs args);

        public delegate void LoginHandler(DiscordSocketClient client, LoginEventArgs args);
        public event LoginHandler OnLoggedIn;

        public delegate void LogoutHandler(DiscordSocketClient client, LogoutEventArgs args);
        public event LogoutHandler OnLoggedOut;

        public delegate void SettingsHandler(DiscordSocketClient client, DiscordSettingsEventArgs args);
        public event SettingsHandler OnSettingsUpdated;
        
        public delegate void SessionsHandler(DiscordSocketClient client, DiscordSessionsEventArgs args);
        public event SessionsHandler OnSessionsUpdated;

        public event UserHandler OnUserUpdated;

        public delegate void SocketGuildHandler(DiscordSocketClient client, SocketGuildEventArgs args);
        public event SocketGuildHandler OnJoinedGuild;

        public delegate void GuildUpdateHandler(DiscordSocketClient client, GuildEventArgs args);
        public event GuildUpdateHandler OnGuildUpdated;

        public delegate void GuildUnavailableHandler(DiscordSocketClient client, GuildUnavailableEventArgs args);
        public event GuildUnavailableHandler OnLeftGuild;

        public delegate void MemberAddedHandler(DiscordSocketClient client, GuildMemberEventArgs args);
        public event MemberAddedHandler OnUserJoinedGuild;

        public delegate void MemberRemovedHandler(DiscordSocketClient client, MemberRemovedEventArgs args);
        public event MemberRemovedHandler OnUserLeftGuild;

        public delegate void GuildMemberHandler(DiscordSocketClient client, GuildMemberEventArgs args);
        public event GuildMemberHandler OnGuildMemberUpdated;

        internal delegate void GuildMembersHandler(DiscordSocketClient client, GuildMembersEventArgs args);
        internal event GuildMembersHandler OnGuildMembersReceived;

        internal delegate void MemberListHandler(DiscordSocketClient client, GuildMemberListEventArgs args);
        internal event MemberListHandler OnMemberListUpdate;

        public delegate void PresenceUpdateHandler(DiscordSocketClient client, PresenceUpdatedEventArgs args);
        public event PresenceUpdateHandler OnUserPresenceUpdated;

        public delegate void GiftHandler(DiscordSocketClient client, GiftCodeCreatedEventArgs args);
        public event GiftHandler OnGiftCodeCreated;

        public delegate void BoostHandler(DiscordSocketClient client, NitroBoostUpdatedEventArgs args);
        public event BoostHandler OnBoostUpdated;

        public event RoleHandler OnRoleCreated;
        public event RoleHandler OnRoleUpdated;

        public delegate void RoleDeleteHandler(DiscordSocketClient client, RoleDeletedEventArgs args);
        public event RoleDeleteHandler OnRoleDeleted;

        public event ChannelHandler OnChannelCreated;
        public event ChannelHandler OnChannelUpdated;
        public event ChannelHandler OnChannelDeleted;

        public delegate void RingingHandler(DiscordSocketClient client, RingingEventArgs args);
        public event RingingHandler OnRinging;

        public delegate void CallUpdateHandler(DiscordSocketClient client, CallUpdateEventArgs args);
        public event CallUpdateHandler OnCallUpdated;

        public delegate void CallEndedHandler(DiscordSocketClient client, ulong channelId);
        public event CallEndedHandler OnCallEnded;

        public delegate void InviteCreateHandler(DiscordSocketClient client, InviteCreatedEventArgs args);
        public event InviteCreateHandler OnInviteCreated;

        public delegate void InviteDeleteHandler(DiscordSocketClient client, InviteDeletedEventArgs args);
        public event InviteDeleteHandler OnInviteDeleted;

        public event VoiceStateHandler OnVoiceStateUpdated;

        internal delegate void VoiceServerHandler(DiscordSocketClient client, DiscordVoiceServer server);
        internal event VoiceServerHandler OnVoiceServer;

        public delegate void EmojisUpdatedHandler(DiscordSocketClient client, EmojisUpdatedEventArgs args);
        public event EmojisUpdatedHandler OnEmojisUpdated;

        public delegate void UserTypingHandler(DiscordSocketClient client, UserTypingEventArgs args);
        public event UserTypingHandler OnUserTyping;

        public event MessageHandler OnMessageReceived;
        public event MessageHandler OnMessageEdited;
        public delegate void MessageDeletedHandler(DiscordSocketClient client, MessageDeletedEventArgs args);
        public event MessageDeletedHandler OnMessageDeleted;

        public event ReactionHandler OnMessageReactionAdded;
        public event ReactionHandler OnMessageReactionRemoved;

        public event BanUpdateHandler OnUserBanned;
        public event BanUpdateHandler OnUserUnbanned;

        public event RelationshipHandler OnRelationshipAdded;
        public event RelationshipHandler OnRelationshipRemoved;
        #endregion
        
        // caching
        internal Dictionary<ulong, SocketGuild> GuildCache { get; private set; }
        internal List<PrivateChannel> PrivateChannels { get; private set; }
        internal Dictionary<ulong, DiscordVoiceState> VoiceStates { get; private set; }
        internal Dictionary<ulong, ClientGuildSettings> GuildSettings { get; private set; }
        internal List<DiscordChannelSettings> PrivateChannelSettings { get; private set; }

        public CommandHandler CommandHandler { get; private set; }
        public new DiscordSocketConfig Config { get; private set; }
        internal List<VoiceSessionInfo> VoiceSessions { get; private set; }

        public DiscordUserSettings UserSettings { get; private set; }

        internal ulong? Lurking { get; set; }

        // websocket connection
        internal WebSocket Socket { get; private set; }
        public bool LoggedIn { get; private set; }
        internal uint? Sequence { get; set; }
        public string SessionId { get; set; }

        internal DateTime Cooldown { get; set; }
        internal object RequestLock { get; private set; } = new object();

        

        public DiscordSocketClient(DiscordSocketConfig config = null) : base(config)
        {
            if (config == null)
                config = new DiscordSocketConfig();

            Config = config;

            if (Config.Cache)
            {
                GuildCache = new Dictionary<ulong, SocketGuild>();
                PrivateChannels = new List<PrivateChannel>();
                VoiceStates = new Dictionary<ulong, DiscordVoiceState>();
                GuildSettings = new Dictionary<ulong, ClientGuildSettings>();
                PrivateChannelSettings = new List<DiscordChannelSettings>();
            }

            VoiceSessions = new List<VoiceSessionInfo>();
        }

        ~DiscordSocketClient()
        {
            Dispose(true);
        }


        public void Login(string token)
        {
            if (Token != token)
                Token = token;

            Socket = new WebSocket("wss://gateway.discord.gg/?v=6&encoding=json");
            if (Config.Proxy != null && Config.Proxy.Type == ProxyType.HTTP) //WebSocketSharp only supports HTTP proxies :(
                Socket.SetProxy("http://" + Config.Proxy.Host + Config.Proxy.Port, Config.Proxy.Username, Config.Proxy.Password);
            Socket.OnMessage += SocketDataReceived;
            Socket.OnClose += (sender, e) => 
            {
                Reset();

                if (e.Code >= (ushort)GatewayCloseError.UnknownError)
                {
                    GatewayCloseError err = (GatewayCloseError)e.Code;

                    if (err != GatewayCloseError.RateLimited && err != GatewayCloseError.SessionTimedOut && err != GatewayCloseError.UnknownError)
                    {
                        if (LoggedIn)
                        {
                            LoggedIn = false;

                            OnLoggedOut?.Invoke(this, new LogoutEventArgs(err));
                        }
                    }
                }

                if (LoggedIn)
                {
                    while (true)
                    {
                        try
                        {
                            Login(Token);

                            return;
                        }
                        catch
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            };

            Socket.Connect();
        }


        public void Logout()
        {
            if (LoggedIn)
            {
                LoggedIn = false;

                Socket.Close();

                OnLoggedOut?.Invoke(this, new LogoutEventArgs());
            }
        }


        public void CreateCommandHandler(string prefix)
        {
            CommandHandler = new CommandHandler(prefix, this);
        }


        private void Reset()
        {
            SessionId = null;

            foreach (var voiceSession in VoiceSessions)
                voiceSession.Session.Disconnect();

            VoiceSessions.Clear();

            if (Config.Cache)
            {
                GuildCache.Clear();
                PrivateChannels.Clear();
                VoiceStates.Clear();
                GuildSettings.Clear();
                PrivateChannelSettings.Clear();
            }
        }


        private void SocketDataReceived(object sender, WebSocketSharp.MessageEventArgs result)
        {
            GatewayResponse payload = result.Data.Deserialize<GatewayResponse>();
            Sequence = payload.Sequence;

            try
            {
                switch (payload.Opcode)
                {
                    case GatewayOpcode.Event:
                        /*
                        Console.WriteLine(payload.Title);
                        
                        File.AppendAllText("Debug.log", $"{payload.Title}: {payload.Data}\n");
                        */
                        switch (payload.Title)
                        {
                            case "READY":
                                Login login = payload.DeserializeEx<Login>().SetClient(this);

                                this.User = login.User;
                                this.UserSettings = login.Settings;
                                this.SessionId = login.SessionId;

                                if (Config.Cache)
                                {
                                    if (this.User.Type == DiscordUserType.User)
                                    {
                                        PrivateChannels = login.PrivateChannels;

                                        foreach (var guild in login.Guilds)
                                        {
                                            GuildCache[guild.Id] = guild.ToSocketGuild();

                                            foreach (var state in GuildCache[guild.Id].VoiceStates)
                                                VoiceStates[state.UserId] = state;
                                        }

                                        foreach (var settings in login.ClientGuildSettings)
                                        {
                                            if (settings.GuildId.HasValue)
                                                GuildSettings.Add(settings.Guild.Id, settings);
                                            else
                                                PrivateChannelSettings = settings.ChannelOverrides.ToList();
                                        }
                                    }
                                }

                                LoggedIn = true;

                                Task.Run(() => OnLoggedIn?.Invoke(this, new LoginEventArgs(login)));
                                break;
                            case "USER_SETTINGS_UPDATE":
                                if (UserSettings != null) // for some reason this is null sometimes :thinking:
                                {
                                    var update = payload.Deserialize<JObject>();

                                    foreach (var field in UserSettings.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                                    {
                                        foreach (var attr in field.CustomAttributes)
                                        {
                                            if (attr.AttributeType == typeof(JsonPropertyAttribute))
                                            {
                                                string propertyName = attr.ConstructorArguments[0].Value.ToString();

                                                if (update.ContainsKey(propertyName))
                                                    field.SetValue(UserSettings, update.GetValue(propertyName).ToObject(field.FieldType));

                                                break;
                                            }
                                        }
                                    }

                                    foreach (var property in UserSettings.GetType().GetProperties())
                                    {
                                        foreach (var attr in property.CustomAttributes)
                                        {
                                            if (attr.AttributeType == typeof(JsonPropertyAttribute))
                                            {
                                                string propertyName = attr.ConstructorArguments[0].Value.ToString();

                                                if (update.ContainsKey(propertyName))
                                                    property.SetValue(UserSettings, update.GetValue(propertyName).ToObject(property.PropertyType));

                                                break;
                                            }
                                        }
                                    }

                                    Task.Run(() => OnSettingsUpdated?.Invoke(this, new DiscordSettingsEventArgs(UserSettings)));
                                }
                                break;
                            case "USER_GUILD_SETTINGS_UPDATE":
                                if (Config.Cache)
                                {
                                    ClientGuildSettings settings = payload.Deserialize<ClientGuildSettings>();

                                    if (settings.GuildId.HasValue)
                                        GuildSettings[settings.Guild.Id] = settings;
                                    else
                                        PrivateChannelSettings = settings.ChannelOverrides.ToList();
                                }
                                break;
                            case "USER_UPDATE":
                                DiscordUser user = payload.Deserialize<DiscordUser>().SetClient(this);

                                if (user.Id == User.Id)
                                    User.Update(user);

                                if (Config.Cache)
                                {
                                    foreach (var dm in PrivateChannels)
                                    {
                                        bool updated = false;

                                        foreach (var recipient in dm.Recipients)
                                        {
                                            if (recipient.Id == user.Id)
                                            {
                                                recipient.Update(user);

                                                updated = true;

                                                break;
                                            }
                                        }

                                        if (updated) // this is somewhat resource intensive, so let's reduce the uses as much as possible
                                            dm.UpdateSelfJson();
                                    }
                                }

                                Task.Run(() => OnUserUpdated?.Invoke(this, new UserEventArgs(user)));
                                break;
                            case "GUILD_MEMBER_LIST_UPDATE":
                                OnMemberListUpdate?.Invoke(this, payload.Deserialize<GuildMemberListEventArgs>());
                                break;
                            case "GUILD_CREATE":
                                {
                                    SocketGuild guild = payload.DeserializeEx<SocketGuild>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        GuildCache.Remove(guild.Id);

                                        GuildCache.Add(guild.Id, guild);
                                    }

                                    Task.Run(() => OnJoinedGuild?.Invoke(this, new SocketGuildEventArgs(guild, Lurking.HasValue && Lurking.Value == guild.Id)));
                                }
                                break;
                            case "GUILD_UPDATE":
                                {
                                    DiscordGuild guild = payload.Deserialize<DiscordGuild>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[guild].Update(guild);

                                    Task.Run(() => OnGuildUpdated?.Invoke(this, new GuildEventArgs(guild)));
                                }
                                break;
                            case "GUILD_DELETE":
                                {
                                    UnavailableGuild guild = payload.Deserialize<UnavailableGuild>();

                                    if (Lurking.HasValue && Lurking.Value == guild.Id)
                                        Lurking = null;

                                    if (Config.Cache)
                                    {
                                        if (guild.Unavailable)
                                            GuildCache[guild.Id].Unavailable = true;
                                        else
                                            GuildCache.Remove(guild.Id);

                                        GuildSettings.Remove(guild.Id);
                                    }

                                    Task.Run(() => OnLeftGuild?.Invoke(this, new GuildUnavailableEventArgs(guild)));
                                }
                                break;
                            case "GUILD_MEMBER_ADD":
                                Task.Run(() => OnUserJoinedGuild?.Invoke(this, new GuildMemberEventArgs(payload.Deserialize<GuildMember>().SetClient(this))));
                                break;
                            case "GUILD_MEMBER_REMOVE":
                                Task.Run(() => OnUserLeftGuild?.Invoke(this, new MemberRemovedEventArgs(payload.Deserialize<PartialGuildMember>().SetClient(this))));
                                break;
                            case "GUILD_MEMBER_UPDATE":
                                GuildMember member = payload.Deserialize<GuildMember>().SetClient(this);

                                if (Config.Cache && member.User.Id == User.Id)
                                {
                                    SocketGuild guild = this.GetCachedGuild(member.GuildId);

                                    // Discord doesn't send us the user's JoinedAt on updates
                                    member.JoinedAt = guild.Member.JoinedAt;
                                    guild.Member = member;

                                    break;
                                }

                                Task.Run(() => OnGuildMemberUpdated?.Invoke(this, new GuildMemberEventArgs(member)));
                                break;
                            case "GUILD_MEMBERS_CHUNK":
                                Task.Run(() => OnGuildMembersReceived?.Invoke(this, new GuildMembersEventArgs(payload.Deserialize<GuildMemberList>().SetClient(this))));
                                break;
                            case "GIFT_CODE_CREATE":
                                Task.Run(() => OnGiftCodeCreated?.Invoke(this, payload.Deserialize<GiftCodeCreatedEventArgs>()));
                                break;
                            case "PRESENCE_UPDATE":
                                Task.Run(() => OnUserPresenceUpdated?.Invoke(this, new PresenceUpdatedEventArgs(payload.DeserializeEx<DiscordPresence>().SetClient(this))));
                                break;
                            case "VOICE_STATE_UPDATE":
                                DiscordVoiceState newState = payload.Deserialize<DiscordVoiceState>().SetClient(this);

                                if (Config.Cache)
                                {
                                    // this doesn't work very well for bot accounts since those can be connected to a channel in multiple guilds at once.
                                    VoiceStates[newState.UserId] = newState;

                                    // we also store voice states within SocketGuilds, so make sure to update those.
                                    foreach (var guild in this.GetCachedGuilds())
                                    {
                                        if (newState.Guild == null || guild.Id != newState.Guild.Id)
                                            guild._voiceStates.RemoveAll(s => s.UserId == newState.UserId);
                                        else
                                        {
                                            int i = guild._voiceStates.FindIndex(s => s.UserId == newState.UserId);

                                            if (i > -1)
                                                guild._voiceStates[i] = newState;
                                            else
                                                guild._voiceStates.Add(newState);
                                        }
                                    }
                                }

                                Task.Run(() => OnVoiceStateUpdated?.Invoke(this, new VoiceStateEventArgs(newState)));
                                break;
                            case "VOICE_SERVER_UPDATE":
                                Task.Run(() => OnVoiceServer?.Invoke(this, payload.Deserialize<DiscordVoiceServer>()));
                                break;
                            case "GUILD_ROLE_CREATE":
                                {
                                    DiscordRole role = payload.Deserialize<RoleUpdate>().Role.SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[role.GuildId]._roles.Add(role);

                                    Task.Run(() => OnRoleCreated?.Invoke(this, new RoleEventArgs(role)));
                                }
                                break;
                            case "GUILD_ROLE_UPDATE":
                                {
                                    DiscordRole role = payload.Deserialize<RoleUpdate>().Role.SetClient(this);

                                    if (Config.Cache)
                                    {
                                        var roles = GuildCache[role.GuildId]._roles;
                                        roles[roles.FindIndex(r => r.Id == role.Id)] = role;
                                    }
                                    
                                    Task.Run(() => OnRoleUpdated?.Invoke(this, new RoleEventArgs(role)));
                                }
                                break;
                            case "GUILD_ROLE_DELETE":
                                {
                                    DeletedRole role = payload.Deserialize<DeletedRole>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[role.Guild]._roles.RemoveAll(r => r.Id == role.Id);

                                    Task.Run(() => OnRoleDeleted?.Invoke(this, new RoleDeletedEventArgs(role)));
                                }
                                break;
                            case "GUILD_EMOJIS_UPDATE":
                                var emojis = payload.Deserialize<EmojiContainer>().SetClient(this);

                                if (Config.Cache)
                                    GuildCache[emojis.GuildId]._emojis = emojis.Emojis.ToList();

                                Task.Run(() => OnEmojisUpdated?.Invoke(this, new EmojisUpdatedEventArgs(emojis)));
                                break;
                            case "CHANNEL_CREATE":
                                {
                                    var channel = payload.DeserializeEx<DiscordChannel>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels.Add(channel.ToDMChannel());
                                        else
                                        {
                                            GuildChannel guildChannel = channel.ToGuildChannel();

                                            GuildCache[guildChannel.GuildId]._channels.Add(guildChannel);
                                        }
                                    }

                                    Task.Run(() => OnChannelCreated?.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "CHANNEL_UPDATE":
                                {
                                    var channel = payload.DeserializeEx<DiscordChannel>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels[PrivateChannels.FindIndex(c => c.Id == channel.Id)] = channel.ToDMChannel();
                                        else
                                        {
                                            GuildChannel guildChannel = channel.ToGuildChannel();

                                            var channels = GuildCache[guildChannel.GuildId]._channels;

                                            channels[channels.FindIndex(c => c.Id == guildChannel.Id)] = guildChannel;
                                        }
                                    }

                                    Task.Run(() => OnChannelUpdated?.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "CHANNEL_DELETE":
                                {
                                    var channel = payload.DeserializeEx<DiscordChannel>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels.RemoveAll(c => c.Id == channel.Id);
                                        else
                                            GuildCache[channel.ToGuildChannel().GuildId]._channels.RemoveAll(c => c.Id == channel.Id);
                                    }

                                    Task.Run(() => OnChannelDeleted?.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "TYPING_START":
                                Task.Run(() => OnUserTyping?.Invoke(this, new UserTypingEventArgs(payload.Deserialize<UserTyping>())));
                                break;
                            case "MESSAGE_CREATE":
                                var message = payload.Deserialize<DiscordMessage>().SetClient(this);

                                if (Config.Cache)
                                {
                                    var channel = this.GetChannel(message.Channel.Id);

                                    channel.Json["last_message_id"] = JToken.FromObject(message.Id);
                                }

                                Task.Run(() => OnMessageReceived?.Invoke(this, new MessageEventArgs(message)));
                                break;
                            case "MESSAGE_UPDATE":
                                Task.Run(() => OnMessageEdited?.Invoke(this, new MessageEventArgs(payload.Deserialize<DiscordMessage>().SetClient(this))));
                                break;
                            case "MESSAGE_DELETE":
                                Task.Run(() => OnMessageDeleted?.Invoke(this, new MessageDeletedEventArgs(payload.Deserialize<DeletedMessage>())));
                                break;
                            case "MESSAGE_REACTION_ADD":
                                Task.Run(() => OnMessageReactionAdded?.Invoke(this, new ReactionEventArgs(payload.Deserialize<MessageReactionUpdate>().SetClient(this))));
                                break;
                            case "MESSAGE_REACTION_REMOVE":
                                Task.Run(() => OnMessageReactionRemoved?.Invoke(this, new ReactionEventArgs(payload.Deserialize<MessageReactionUpdate>().SetClient(this))));
                                break;
                            case "GUILD_BAN_ADD":
                                Task.Run(() => OnUserBanned?.Invoke(this, new BanUpdateEventArgs(payload.Deserialize<BanContainer>().SetClient(this))));
                                break;
                            case "GUILD_BAN_REMOVE":
                                Task.Run(() => OnUserUnbanned?.Invoke(this, new BanUpdateEventArgs(payload.Deserialize<BanContainer>().SetClient(this))));
                                break;
                            case "INVITE_CREATE":
                                Task.Run(() => OnInviteCreated?.Invoke(this, payload.Deserialize<InviteCreatedEventArgs>()));
                                break;
                            case "INVITE_DELETE":
                                Task.Run(() => OnInviteDeleted?.Invoke(this, payload.Deserialize<InviteDeletedEventArgs>()));
                                break;
                            case "RELATIONSHIP_ADD":
                                Task.Run(() => OnRelationshipAdded?.Invoke(this, new RelationshipEventArgs(payload.Deserialize<Relationship>().SetClient(this))));
                                break;
                            case "RELATIONSHIP_REMOVE":
                                Task.Run(() => OnRelationshipRemoved?.Invoke(this, new RelationshipEventArgs(payload.Deserialize<Relationship>().SetClient(this))));
                                break;
                            case "CHANNEL_RECIPIENT_ADD":
                                {
                                    var recipUpdate = payload.Deserialize<ChannelRecipientUpdate>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        foreach (var channel in PrivateChannels)
                                        {
                                            if (channel.Id == recipUpdate.Channel.Id)
                                            {
                                                channel._recipients.Add(recipUpdate.User);

                                                channel.UpdateSelfJson();

                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "CHANNEL_RECIPIENT_REMOVE":
                                {
                                    var recipUpdate = payload.Deserialize<ChannelRecipientUpdate>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        foreach (var channel in PrivateChannels)
                                        {
                                            if (channel.Id == recipUpdate.Channel.Id)
                                            {
                                                channel._recipients.RemoveAll(u => u.Id == recipUpdate.User.Id);

                                                channel.UpdateSelfJson();

                                                break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "MESSAGE_ACK": // triggered whenever another person logged into the account acknowledges a message
                                break;
                            case "SESSIONS_REPLACE":
                                Task.Run(() => OnSessionsUpdated?.Invoke(this, new DiscordSessionsEventArgs(payload.Deserialize<List<DiscordSession>>())));
                                break;
                            case "CALL_CREATE":
                                {
                                    JObject obj = payload.Deserialize<JObject>();

                                    var call = obj.ToObject<DiscordCall>().SetClient(this);
                                    var voiceStates = obj.Value<JToken>("voice_states").ToObject<IReadOnlyList<DiscordVoiceState>>().SetClientsInList(this);

                                    foreach (var state in voiceStates)
                                        VoiceStates[state.UserId] = state;

                                    Task.Run(() => OnRinging?.Invoke(this, new RingingEventArgs(call, voiceStates)));
                                }
                                break;
                            case "CALL_UPDATE":
                                Task.Run(() => OnCallUpdated?.Invoke(this, new CallUpdateEventArgs(payload.Deserialize<DiscordCall>().SetClient(this))));
                                break;
                            case "CALL_DELETE":
                                ulong channelId = payload.Deserialize<JObject>().Value<ulong>("channel_id");

                                foreach (var state in new Dictionary<ulong, DiscordVoiceState>.ValueCollection(VoiceStates))
                                {
                                    if (state.Channel != null && state.Channel.Id == channelId)
                                        VoiceStates.Remove(state.UserId);
                                }

                                Task.Run(() => OnCallEnded?.Invoke(this, channelId));
                                break;
                            case "USER_PREMIUM_GUILD_SUBSCRIPTION_SLOT_UPDATE":
                                Task.Run(() => OnBoostUpdated?.Invoke(this, new NitroBoostUpdatedEventArgs(payload.Deserialize<DiscordNitroBoost>().SetClient(this))));
                                break;
                        }
                        break;
                    case GatewayOpcode.InvalidSession:
                        LoggedIn = false;

                        this.LoginToGateway();
                        break;
                    case GatewayOpcode.Connected:
                        this.LoginToGateway();

                        Task.Run(() =>
                        {
                            int interval = payload.Deserialize<JObject>().GetValue("heartbeat_interval").ToObject<int>();

                            try
                            {
                                while (true)
                                {
                                    this.Send(GatewayOpcode.Heartbeat, this.Sequence);
                                    Thread.Sleep(interval);
                                }
                            }
                            catch { }
                        });
                        
                        break;
                }
            }
            catch 
            {
            }
        }

        internal void Dispose(bool destructor)
        {
            Logout();

            if (!destructor)
                Reset();
        }


        public void Dispose()
        {
            Dispose(false);
        }
    }
}