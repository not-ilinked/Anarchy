using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using System.IO;
using Discord.Media;
using Discord.WebSockets;
using Anarchy;
using Newtonsoft.Json;

namespace Discord.Gateway
{
    /// <summary>
    /// <see cref="DiscordClient"/> with Gateway support
    /// </summary>
    public class DiscordSocketClient : DiscordClient, IDisposable
    {
        #region events
        public delegate void ChannelHandler(DiscordSocketClient client, ChannelEventArgs args);
        public delegate void MessageHandler(DiscordSocketClient client, MessageEventArgs args);
        public delegate void ReactionHandler(DiscordSocketClient client, ReactionEventArgs args);
        public delegate void RoleHandler(DiscordSocketClient client, RoleEventArgs args);
        public delegate void BanUpdateHandler(DiscordSocketClient client, BanUpdateEventArgs args);
        public delegate void RelationshipHandler(DiscordSocketClient client, RelationshipEventArgs args);
        public delegate void MemberHandler(DiscordSocketClient client, GuildMemberEventArgs args);
        public delegate void RecipientHandler(DiscordSocketClient client, ChannelRecipientEventArgs args);

        public delegate void LoginHandler(DiscordSocketClient client, LoginEventArgs args);
        public event LoginHandler OnLoggedIn;

        public delegate void LogoutHandler(DiscordSocketClient client, LogoutEventArgs args);
        public event LogoutHandler OnLoggedOut;

        public delegate void SettingsHandler(DiscordSocketClient client, DiscordSettingsEventArgs args);
        public event SettingsHandler OnSettingsUpdated;
        
        public delegate void SessionsHandler(DiscordSocketClient client, DiscordSessionsEventArgs args);
        public event SessionsHandler OnSessionsUpdated;

        public delegate void UserHandler(DiscordSocketClient client, UserEventArgs args);
        public event UserHandler OnUserUpdated;

        public delegate void SocketGuildHandler(DiscordSocketClient client, SocketGuildEventArgs args);
        public event SocketGuildHandler OnJoinedGuild;

        public delegate void GuildUpdateHandler(DiscordSocketClient client, GuildEventArgs args);
        public event GuildUpdateHandler OnGuildUpdated;

        public delegate void GuildUnavailableHandler(DiscordSocketClient client, GuildUnavailableEventArgs args);
        public event GuildUnavailableHandler OnLeftGuild;

        public event MemberHandler OnUserJoinedGuild;
        public event MemberHandler OnGuildMemberUpdated;

        public delegate void MemberRemovedHandler(DiscordSocketClient client, MemberRemovedEventArgs args);
        public event MemberRemovedHandler OnUserLeftGuild;

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

        public delegate void VoiceStateHandler(DiscordSocketClient client, VoiceStateEventArgs args);
        public event VoiceStateHandler OnVoiceStateUpdated;

        internal delegate void MediaServerHandler(DiscordSocketClient client, DiscordMediaServer server);
        internal event MediaServerHandler OnMediaServer;

        internal delegate void VoiceSessionHandler(DiscordSocketClient client, DiscordVoiceState state);
        internal event VoiceSessionHandler OnConnectionVoiceState;

        internal delegate void StreamCreateHandler(DiscordSocketClient client, GoLiveCreate stream);
        internal event StreamCreateHandler OnStreamCreated;

        internal delegate void StreamUpdateHandler(DiscordSocketClient client, GoLiveUpdate goLive);
        internal event StreamUpdateHandler OnStreamUpdated;

        internal delegate void StreamDeleteHandler(DiscordSocketClient client, GoLiveDelete goLive);
        internal event StreamDeleteHandler OnStreamDeleted;

        public delegate void EmojisUpdatedHandler(DiscordSocketClient client, EmojisUpdatedEventArgs args);
        public event EmojisUpdatedHandler OnEmojisUpdated;

        public delegate void UserTypingHandler(DiscordSocketClient client, UserTypingEventArgs args);
        public event UserTypingHandler OnUserTyping;

        public event MessageHandler OnMessageReceived;
        public event MessageHandler OnMessageEdited;

        public delegate void MessageDeletedHandler(DiscordSocketClient client, MessageDeletedEventArgs args);
        public event MessageDeletedHandler OnMessageDeleted;

        public delegate void ChannelUnreadHandler(DiscordSocketClient client, UnreadMessagesEventArgs args);
        public event ChannelUnreadHandler OnGuildUnreadMessagesUpdated;

        public event ReactionHandler OnMessageReactionAdded;
        public event ReactionHandler OnMessageReactionRemoved;

        public event BanUpdateHandler OnUserBanned;
        public event BanUpdateHandler OnUserUnbanned;

        public event RelationshipHandler OnRelationshipAdded;
        public event RelationshipHandler OnRelationshipRemoved;

        public event RecipientHandler OnChannelRecipientAdded;
        public event RecipientHandler OnChannelRecipientRemoved;
        #endregion
        
        // caching
        internal ConcurrentDictionary<ulong, SocketGuild> GuildCache { get; private set; }
        internal ConcurrentList<PrivateChannel> PrivateChannels { get; private set; }
        internal AutoConcurrentDictionary<ulong, DiscordVoiceStateContainer> VoiceStates { get; private set; }
        internal ConcurrentDictionary<ulong, ClientGuildSettings> GuildSettings { get; private set; }
        internal List<DiscordChannelSettings> PrivateChannelSettings { get; private set; }

        public CommandHandler CommandHandler { get; private set; }
        public new LockedSocketConfig Config { get; private set; }
        internal ConcurrentDictionary<ulong, DiscordVoiceSession> VoiceSessions { get; private set; }
        internal ConcurrentDictionary<string, DiscordGoLiveSession> Livestreams { get; private set; }

        public DiscordUserSettings UserSettings { get; private set; }

        // websocket connection
        internal DiscordWebSocket<GatewayOpcode> WebSocket { get; private set; }
        public GatewayConnectionState State { get; private set; }
        public bool LoggedIn { get; private set; }
        internal uint? Sequence { get; set; }
        public string SessionId { get; set; }

        internal DateTime Cooldown { get; set; }
        internal object RequestLock { get; private set; }
        internal ulong? Lurking { get; set; }


        public DiscordSocketClient(DiscordSocketConfig config = null) : base()
        {
            RequestLock = new object();

            if (config == null)
                config = new DiscordSocketConfig();

            Config = new LockedSocketConfig(config);
            base.Config = Config;

            FinishConfig();

            if (Config.Cache)
            {
                GuildCache = new ConcurrentDictionary<ulong, SocketGuild>();
                PrivateChannels = new ConcurrentList<PrivateChannel>();
                VoiceStates = new AutoConcurrentDictionary<ulong, DiscordVoiceStateContainer>((userId) => new DiscordVoiceStateContainer(userId));
                GuildSettings = new ConcurrentDictionary<ulong, ClientGuildSettings>();
                PrivateChannelSettings = new List<DiscordChannelSettings>();
            }

            VoiceSessions = new ConcurrentDictionary<ulong, DiscordVoiceSession>();
            Livestreams = new ConcurrentDictionary<string, DiscordGoLiveSession>();

            WebSocket = new DiscordWebSocket<GatewayOpcode>($"wss://gateway.discord.gg/?v={Config.ApiVersion}&encoding=json");

            WebSocket.OnClosed += (s, args) =>
            {
                State = GatewayConnectionState.NotConnected;

                Reset();

                bool lostConnection = args.Code == 1006 || args.Code == 1001;

                if (lostConnection)
                    Thread.Sleep(200);

                GatewayCloseCode err = (GatewayCloseCode)args.Code;

                if (LoggedIn && (lostConnection || err == GatewayCloseCode.RateLimited || err == GatewayCloseCode.SessionTimedOut || err == GatewayCloseCode.UnknownError))
                    Login(Token);
                else
                    OnLoggedOut?.Invoke(this, new LogoutEventArgs(err, args.Reason));
            };

            WebSocket.OnMessageReceived += WebSocket_OnMessageReceived;

            #region media event handlers
            OnConnectionVoiceState += (c, state) =>
            {
                lock (VoiceSessions.Lock)
                {
                    foreach (var session in VoiceSessions.Values)
                    {
                        if (session.SessionId == state.SessionId)
                        {
                            if (state.Channel == null)
                                session.Disconnect(DiscordMediaCloseCode.Disconnected, "Disconnected."); // pretty sure this is the 'reason' discord normally sends?
                            else
                                session.ChannelId = state.Channel.Id;

                            break;
                        }
                    }
                }
            };

            OnMediaServer += (c, args) =>
            {
                if (args.StreamKey == null)
                {
                    lock (VoiceSessions.Lock)
                    {
                        foreach (var session in VoiceSessions.Values)
                        {
                            if (args.GuildId == session.GuildId)
                            {
                                session.UpdateServer(args);

                                break;
                            }
                        }
                    }
                }
                else if (Livestreams.TryGetValue(args.StreamKey, out DiscordGoLiveSession session))
                {
                    args.GuildId = session.Guild.Id;
                    session.UpdateServer(args);

                    if (args.StreamKey.Split(':').Last() == User.Id.ToString())
                        session.ParentSession.Livestream = session;
                    else
                        session.ParentSession.WatchingDictionary[args.StreamKey] = session;
                }
            };

            OnStreamUpdated += (c, update) =>
            {
                if (Livestreams.TryGetValue(update.StreamKey, out DiscordGoLiveSession session))
                    session.Update(update);
            };

            OnStreamDeleted += (c, delete) =>
            {
                if (Livestreams.TryGetValue(delete.StreamKey, out DiscordGoLiveSession session))
                {
                    Livestreams.Remove(delete.StreamKey);

                    if (delete.StreamKey.Split(':').Last() == User.Id.ToString())
                        session.ParentSession.Livestream = null;
                    else
                        session.ParentSession.WatchingDictionary.Remove(delete.StreamKey);

                    session.Disconnect(delete);
                }
            };
            #endregion
        }

        ~DiscordSocketClient()
        {
            Dispose(true);
        }


        public void Login(string token)
        {
            if (Token != token)
                Token = token;

            if (User.Type == DiscordUserType.Bot && Config.ApiVersion >= 8 && !Config.Intents.HasValue)
                throw new ArgumentNullException("Intents must be supplied as of API v8");

            State = GatewayConnectionState.Connecting;

            WebSocket.SetProxy(Proxy);
            WebSocket.Connect();
        }


        public void Send<T>(GatewayOpcode op, T requestData)
        {
            lock (RequestLock)
            {
                if (Cooldown > DateTime.Now)
                    Thread.Sleep(Cooldown - DateTime.Now);

                WebSocket.Send(op, requestData);

                Cooldown = DateTime.Now + new TimeSpan(0, 0, 0, 0, 500);
            }
        }


        private void WebSocket_OnMessageReceived(object sender, DiscordWebSocketMessage<GatewayOpcode> message)
        {
            Sequence = message.Sequence;

            try
            {
                switch (message.Opcode)
                {
                    case GatewayOpcode.Event:
                        /*
                        Console.WriteLine(message.EventName);
                        
                        File.AppendAllText("Debug.log", $"{message.EventName}: {message.Data}\n");
                        */     
                        
                        switch (message.EventName)
                        {
                            case "READY":
                                Login login = message.Data.ToObject<Login>().SetClient(this);

                                this.User = login.User;
                                this.UserSettings = login.Settings;
                                this.SessionId = login.SessionId;

                                if (Config.Cache && this.User.Type == DiscordUserType.User)
                                {
                                    PrivateChannels = new ConcurrentList<PrivateChannel>(login.PrivateChannels);

                                    foreach (var guild in login.Guilds)
                                    {
                                        var socketGuild = GuildCache[guild.Id] = (SocketGuild)guild;

                                        if (!socketGuild.Unavailable)
                                        {
                                            foreach (var state in socketGuild.VoiceStates)
                                                VoiceStates[state.UserId].GuildStates[guild.Id] = state;
                                        }
                                    }

                                    foreach (var settings in login.ClientGuildSettings)
                                    {
                                        if (settings.GuildId.HasValue)
                                            GuildSettings.Add(settings.Guild.Id, settings);
                                        else
                                            PrivateChannelSettings = settings.ChannelOverrides.ToList();
                                    }
                                }

                                LoggedIn = true;
                                State = GatewayConnectionState.Connected;

                                if (OnLoggedIn != null)
                                    Task.Run(() => OnLoggedIn.Invoke(this, new LoginEventArgs(login)));
                                break;
                            case "USER_SETTINGS_UPDATE":
                                if (UserSettings == null)
                                {

                                }

                                UserSettings.Update((JObject)message.Data);

                                if (OnSettingsUpdated != null)
                                    Task.Run(() => OnSettingsUpdated.Invoke(this, new DiscordSettingsEventArgs(UserSettings)));
                                break;
                            case "USER_GUILD_SETTINGS_UPDATE":
                                if (Config.Cache)
                                {
                                    ClientGuildSettings settings = message.Data.ToObject<ClientGuildSettings>();

                                    if (settings.GuildId.HasValue)
                                        GuildSettings[settings.Guild.Id] = settings;
                                    else
                                        PrivateChannelSettings = settings.ChannelOverrides.ToList();
                                }
                                break;
                            case "USER_UPDATE":
                                DiscordUser user = message.Data.ToObject<DiscordUser>().SetClient(this);

                                if (user.Id == User.Id)
                                    User.Update(user);

                                if (Config.Cache)
                                {
                                    lock (PrivateChannels.Lock)
                                    {
                                        foreach (var dm in PrivateChannels)
                                        {
                                            foreach (var recipient in dm.Recipients)
                                            {
                                                if (recipient.Id == user.Id)
                                                {
                                                    recipient.Update(user);

                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (OnUserUpdated != null)
                                    Task.Run(() => OnUserUpdated.Invoke(this, new UserEventArgs(user)));
                                break;
                            case "GUILD_MEMBER_LIST_UPDATE":
                                OnMemberListUpdate?.Invoke(this, message.Data.ToObject<GuildMemberListEventArgs>());
                                break;
                            case "GUILD_CREATE":
                                if (Config.Cache || OnJoinedGuild != null)
                                {
                                    var guild = message.Data.ToObject<SocketGuild>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        GuildCache[guild.Id] = guild;

                                        foreach (var state in guild.VoiceStates)
                                            VoiceStates[state.UserId].GuildStates[guild.Id] = state;
                                    }

                                    if (OnJoinedGuild != null)
                                        Task.Run(() => OnJoinedGuild.Invoke(this, new SocketGuildEventArgs(guild, Lurking.HasValue && Lurking.Value == guild.Id)));
                                }
                                break;
                            case "GUILD_UPDATE":
                                    if (Config.Cache || OnGuildUpdated != null)
                                    {
                                        DiscordGuild guild = message.Data.ToObject<DiscordGuild>().SetClient(this);

                                        if (Config.Cache)
                                            GuildCache[guild.Id].Update(guild);

                                        Task.Run(() => OnGuildUpdated?.Invoke(this, new GuildEventArgs(guild)));
                                    }
                                break;
                            case "GUILD_DELETE":
                                {
                                    UnavailableGuild guild = message.Data.ToObject<UnavailableGuild>();

                                    if (Lurking.HasValue && Lurking.Value == guild.Id)
                                        Lurking = null;

                                    if (Config.Cache)
                                    {
                                        if (guild.Unavailable)
                                            GuildCache[guild.Id].Unavailable = true;
                                        else
                                        {
                                            GuildCache.Remove(guild.Id);
                                            GuildSettings.Remove(guild.Id);
                                        }
                                    }

                                    if (OnLeftGuild != null)
                                        Task.Run(() => OnLeftGuild.Invoke(this, new GuildUnavailableEventArgs(guild)));
                                }
                                break;
                            case "GUILD_MEMBER_ADD":
                                if (Config.Cache || OnUserJoinedGuild != null)
                                {
                                    var member = message.Data.ToObject<GuildMember>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[member.GuildId].MemberCount++;

                                    Task.Run(() => OnUserJoinedGuild?.Invoke(this, new GuildMemberEventArgs(member)));
                                }
                                break;
                            case "GUILD_MEMBER_REMOVE":
                                if (Config.Cache || OnUserLeftGuild != null)
                                {
                                    var member = message.Data.ToObject<PartialGuildMember>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[member.GuildId].MemberCount--;

                                    Task.Run(() => OnUserLeftGuild?.Invoke(this, new MemberRemovedEventArgs(member)));
                                }
                                break;
                            case "GUILD_MEMBER_UPDATE":
                                if (Config.Cache || OnGuildMemberUpdated != null)
                                {
                                    GuildMember member = message.Data.ToObject<GuildMember>().SetClient(this);

                                    if (Config.Cache && member.User.Id == User.Id)
                                    {
                                        SocketGuild guild = this.GetCachedGuild(member.GuildId);

                                        // Discord doesn't send us the user's JoinedAt on updates
                                        member.JoinedAt = guild.ClientMember.JoinedAt;
                                        guild.ClientMember = member;

                                        break;
                                    }

                                    Task.Run(() => OnGuildMemberUpdated?.Invoke(this, new GuildMemberEventArgs(member)));
                                }
                                break;
                            case "GUILD_MEMBERS_CHUNK":
                                Task.Run(() => OnGuildMembersReceived?.Invoke(this, new GuildMembersEventArgs(message.Data.ToObject<GuildMemberList>().SetClient(this))));
                                break;
                            case "GIFT_CODE_CREATE":
                                Task.Run(() => OnGiftCodeCreated?.Invoke(this, message.Data.ToObject<GiftCodeCreatedEventArgs>()));
                                break;
                            case "PRESENCE_UPDATE":
                                if (OnUserPresenceUpdated != null)
                                    Task.Run(() => OnUserPresenceUpdated.Invoke(this, new PresenceUpdatedEventArgs(DeepJsonConverter.ParsePresence((JObject)message.Data).SetClient(this))));
                                break;
                            case "VOICE_STATE_UPDATE":
                                if (Config.Cache || OnConnectionVoiceState != null || OnVoiceStateUpdated != null)
                                {
                                    DiscordVoiceState newState = message.Data.ToObject<DiscordVoiceState>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        if (newState.Guild == null)
                                            VoiceStates[newState.UserId].PrivateChannelVoiceState = newState;
                                        else
                                            VoiceStates[newState.UserId].GuildStates[newState.Guild.Id] = newState;

                                        // we also store voice states within SocketGuilds, so make sure to update those.
                                        foreach (var guild in this.GetCachedGuilds())
                                        {
                                            if (!guild.Unavailable)
                                            { 
                                                if (newState.Guild == null || guild.Id != newState.Guild.Id)
                                                    guild._voiceStates.RemoveFirst(s => s.UserId == newState.UserId);
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
                                    }

                                    if (newState.UserId == User.Id && newState.SessionId == SessionId)
                                        OnConnectionVoiceState?.Invoke(this, newState);

                                    if (OnVoiceStateUpdated != null)
                                        Task.Run(() => OnVoiceStateUpdated.Invoke(this, new VoiceStateEventArgs(newState)));
                                }
                                break;
                            case "VOICE_SERVER_UPDATE":
                                OnMediaServer?.Invoke(this, message.Data.ToObject<DiscordMediaServer>().SetClient(this));
                                break;
                            case "GUILD_ROLE_CREATE":
                                if (Config.Cache || OnRoleCreated != null)
                                {
                                    DiscordRole role = message.Data.ToObject<RoleUpdate>().Role.SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[role.GuildId]._roles.Add(role);

                                    if (OnRoleCreated != null)
                                        Task.Run(() => OnRoleCreated.Invoke(this, new RoleEventArgs(role)));
                                }
                                break;
                            case "GUILD_ROLE_UPDATE":
                                if (Config.Cache || OnRoleUpdated != null) 
                                {
                                    DiscordRole role = message.Data.ToObject<RoleUpdate>().Role.SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[role.GuildId]._roles.ReplaceFirst(r => r.Id == role.Id, role);

                                    if (OnRoleUpdated != null)
                                        Task.Run(() => OnRoleUpdated.Invoke(this, new RoleEventArgs(role)));
                                }
                                break;
                            case "GUILD_ROLE_DELETE":
                                if (Config.Cache || OnRoleDeleted != null)
                                {
                                    DeletedRole role = message.Data.ToObject<DeletedRole>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[role.Guild]._roles.RemoveFirst(r => r.Id == role.Id);

                                    if (OnRoleDeleted != null)
                                        Task.Run(() => OnRoleDeleted.Invoke(this, new RoleDeletedEventArgs(role)));
                                }
                                break;
                            case "GUILD_EMOJIS_UPDATE":
                                if (Config.Cache || OnEmojisUpdated != null)
                                {
                                    var emojis = message.Data.ToObject<EmojiContainer>().SetClient(this);

                                    if (Config.Cache)
                                        GuildCache[emojis.GuildId]._emojis = emojis.Emojis.ToList();

                                    if (OnEmojisUpdated != null)
                                        Task.Run(() => OnEmojisUpdated.Invoke(this, new EmojisUpdatedEventArgs(emojis)));
                                }
                                break;
                            case "CHANNEL_CREATE":
                                if (Config.Cache || OnChannelCreated != null) 
                                {
                                    var channel = ((JObject)message.Data).ParseDeterministic<DiscordChannel>();

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels.Add((PrivateChannel)channel);
                                        else
                                        {
                                            GuildChannel guildChannel = (GuildChannel)channel;

                                            GuildCache[guildChannel.GuildId].ChannelsConcurrent.Add(guildChannel);
                                        }
                                    }

                                    if (OnChannelCreated != null)
                                        Task.Run(() => OnChannelCreated.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "CHANNEL_UPDATE":
                                if (Config.Cache || OnChannelUpdated != null)
                                {
                                    var channel = ((JObject)message.Data).ParseDeterministic<DiscordChannel>();

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels.ReplaceFirst(c => c.Id == channel.Id, (PrivateChannel)channel);
                                        else
                                        {
                                            GuildChannel guildChannel = (GuildChannel)channel;
                                            GuildCache[guildChannel.GuildId].ChannelsConcurrent.ReplaceFirst(c => c.Id == guildChannel.Id, guildChannel);
                                        }
                                    }

                                    if (OnChannelUpdated != null)
                                        Task.Run(() => OnChannelUpdated.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "CHANNEL_DELETE":
                                if (Config.Cache || OnChannelDeleted != null) 
                                {
                                    var channel = ((JObject)message.Data).ParseDeterministic<DiscordChannel>();

                                    if (Config.Cache)
                                    {
                                        if (channel.Type == ChannelType.DM || channel.Type == ChannelType.Group)
                                            PrivateChannels.RemoveFirst(c => c.Id == channel.Id);
                                        else
                                            GuildCache[((GuildChannel)channel).GuildId].ChannelsConcurrent.RemoveFirst(c => c.Id == channel.Id);
                                    }

                                    if (OnChannelDeleted != null)
                                        Task.Run(() => OnChannelDeleted.Invoke(this, new ChannelEventArgs(channel)));
                                }
                                break;
                            case "TYPING_START":
                                if (OnUserTyping != null)
                                    Task.Run(() => OnUserTyping.Invoke(this, new UserTypingEventArgs(message.Data.ToObject<UserTyping>().SetClient(this))));
                                break;
                            case "MESSAGE_CREATE":
                                if (Config.Cache || OnMessageReceived != null)
                                {
                                    var newMessage = message.Data.ToObject<DiscordMessage>().SetClient(this);

                                    if (Config.Cache)
                                        this.GetChannel(newMessage.Channel.Id).SetLastMessageId(newMessage.Id);

                                    if (OnMessageReceived != null)
                                        Task.Run(() => OnMessageReceived.Invoke(this, new MessageEventArgs(newMessage)));
                                }
                                break;
                            case "MESSAGE_UPDATE":
                                if (OnMessageEdited != null)
                                    Task.Run(() => OnMessageEdited.Invoke(this, new MessageEventArgs(message.Data.ToObject<DiscordMessage>().SetClient(this))));
                                break;
                            case "MESSAGE_DELETE":
                                if (OnMessageDeleted != null)
                                    Task.Run(() => OnMessageDeleted.Invoke(this, new MessageDeletedEventArgs(message.Data.ToObject<DeletedMessage>().SetClient(this))));
                                break;
                            case "MESSAGE_REACTION_ADD":
                                if (OnMessageReactionAdded != null)
                                    Task.Run(() => OnMessageReactionAdded.Invoke(this, new ReactionEventArgs(message.Data.ToObject<MessageReactionUpdate>().SetClient(this))));
                                break;
                            case "MESSAGE_REACTION_REMOVE":
                                if (OnMessageReactionRemoved != null)
                                    Task.Run(() => OnMessageReactionRemoved.Invoke(this, new ReactionEventArgs(message.Data.ToObject<MessageReactionUpdate>().SetClient(this))));
                                break;
                            case "GUILD_BAN_ADD":
                                if (OnUserBanned != null)
                                    Task.Run(() => OnUserBanned.Invoke(this, message.Data.ToObject<BanUpdateEventArgs>().SetClient(this)));
                                break;
                            case "GUILD_BAN_REMOVE":
                                if (OnUserUnbanned != null)
                                    Task.Run(() => OnUserUnbanned.Invoke(this, message.Data.ToObject<BanUpdateEventArgs>().SetClient(this)));
                                break;
                            case "INVITE_CREATE":
                                if (OnInviteCreated != null)
                                    Task.Run(() => OnInviteCreated.Invoke(this, message.Data.ToObject<InviteCreatedEventArgs>().SetClient(this)));
                                break;
                            case "INVITE_DELETE":
                                if (OnInviteDeleted != null)
                                    Task.Run(() => OnInviteDeleted.Invoke(this, message.Data.ToObject<InviteDeletedEventArgs>().SetClient(this)));
                                break;
                            case "RELATIONSHIP_ADD":
                                if (OnRelationshipAdded != null)
                                    Task.Run(() => OnRelationshipAdded.Invoke(this, new RelationshipEventArgs(message.Data.ToObject<DiscordRelationship>().SetClient(this))));
                                break;
                            case "RELATIONSHIP_REMOVE":
                                if (OnRelationshipRemoved != null)
                                    Task.Run(() => OnRelationshipRemoved.Invoke(this, new RelationshipEventArgs(message.Data.ToObject<DiscordRelationship>().SetClient(this))));
                                break;
                            case "CHANNEL_RECIPIENT_ADD":
                                if (Config.Cache || OnChannelRecipientAdded != null) 
                                {
                                    var recipUpdate = message.Data.ToObject<ChannelRecipientEventArgs>().SetClient(this);

                                    if (Config.Cache)
                                        ((PrivateChannel)this.GetChannel(recipUpdate.Channel.Id))._recipients.Add(recipUpdate.User);

                                    if (OnChannelRecipientAdded != null)
                                        Task.Run(() => OnChannelRecipientAdded.Invoke(this, recipUpdate));
                                }
                                break;
                            case "CHANNEL_RECIPIENT_REMOVE":
                                if (Config.Cache || OnChannelRecipientAdded != null)
                                {
                                    var recipUpdate = message.Data.ToObject<ChannelRecipientEventArgs>().SetClient(this);

                                    if (Config.Cache)
                                        ((PrivateChannel)this.GetChannel(recipUpdate.Channel.Id))._recipients.RemoveFirst(u => u.Id == recipUpdate.User.Id);

                                    if (OnChannelRecipientRemoved != null)
                                        Task.Run(() => OnChannelRecipientRemoved.Invoke(this, recipUpdate));
                                }
                                break;
                            case "MESSAGE_ACK": // triggered whenever another person logged into the account acknowledges a message
                                break;
                            case "SESSIONS_REPLACE":
                                if (OnSessionsUpdated != null)
                                    Task.Run(() => OnSessionsUpdated.Invoke(this, new DiscordSessionsEventArgs(message.Data.ToObject<List<DiscordSession>>())));
                                break;
                            case "CALL_CREATE":
                                if (Config.Cache || OnRinging != null)
                                {
                                    var call = message.Data.ToObject<DiscordCall>().SetClient(this);
                                    var voiceStates = message.Data.Value<JToken>("voice_states").ToObject<IReadOnlyList<DiscordVoiceState>>().SetClientsInList(this);

                                    if (Config.Cache)
                                    {
                                        foreach (var state in voiceStates)
                                            VoiceStates[state.UserId].PrivateChannelVoiceState = state;
                                    }

                                    if (OnRinging != null)
                                        Task.Run(() => OnRinging.Invoke(this, new RingingEventArgs(call, voiceStates)));
                                }
                                break;
                            case "CALL_UPDATE":
                                if (OnCallUpdated != null)
                                    Task.Run(() => OnCallUpdated.Invoke(this, new CallUpdateEventArgs(message.Data.ToObject<DiscordCall>().SetClient(this))));
                                break;
                            case "CALL_DELETE":
                                if (Config.Cache || OnCallEnded != null)
                                {
                                    ulong channelId = message.Data.Value<ulong>("channel_id");

                                    if (Config.Cache)
                                    {
                                        foreach (var state in VoiceStates.CreateCopy().Values)
                                        {
                                            var privState = state.PrivateChannelVoiceState;

                                            if (privState != null && privState.Channel != null && privState.Channel.Id == channelId)
                                                state.PrivateChannelVoiceState = null;
                                        }
                                    }

                                    if (OnCallEnded != null)
                                        Task.Run(() => OnCallEnded.Invoke(this, channelId));
                                }
                                break;
                            case "USER_PREMIUM_GUILD_SUBSCRIPTION_SLOT_UPDATE":
                                if (OnBoostUpdated != null)
                                    Task.Run(() => OnBoostUpdated.Invoke(this, new NitroBoostUpdatedEventArgs(message.Data.ToObject<DiscordGuildBoost>().SetClient(this))));
                                break;
                            case "STREAM_SERVER_UPDATE":
                                OnMediaServer?.Invoke(this, message.Data.ToObject<DiscordMediaServer>().SetClient(this));
                                break;
                            case "STREAM_CREATE":
                                OnStreamCreated?.Invoke(this, message.Data.ToObject<GoLiveCreate>());
                                break;
                            case "STREAM_UPDATE":
                                OnStreamUpdated?.Invoke(this, message.Data.ToObject<GoLiveUpdate>());
                                break;
                            case "STREAM_DELETE":
                                OnStreamDeleted?.Invoke(this, message.Data.ToObject<GoLiveDelete>());
                                break;
                            case "CHANNEL_UNREAD_UPDATE":
                                if (Config.Cache || OnGuildUnreadMessagesUpdated != null)
                                {
                                    var unread = message.Data.ToObject<GuildUnreadMessages>().SetClient(this);

                                    if (Config.Cache)
                                    {
                                        foreach (var unreadChannel in unread.Channels)
                                            this.GetChannel(unreadChannel.Channel.Id).SetLastMessageId(unreadChannel.LastMessageId);
                                    }

                                    if (OnGuildUnreadMessagesUpdated != null)
                                        Task.Run(() => OnGuildUnreadMessagesUpdated.Invoke(this, new UnreadMessagesEventArgs(unread)));
                                }
                                break;
                            default:
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
                            int interval = message.Data.ToObject<JObject>().GetValue("heartbeat_interval").ToObject<int>();

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

        public void Logout()
        {
            if (LoggedIn)
            {
                LoggedIn = false;

                WebSocket.Close((ushort)GatewayCloseCode.ClosedByClient, "Closed by client");
            }
        }


        public void CreateCommandHandler(string prefix)
        {
            CommandHandler = new CommandHandler(prefix, this);
        }


        private void Reset()
        {
            SessionId = null;

            if (Config.Cache)
            {
                GuildCache.Clear();
                PrivateChannels.Clear();
                VoiceStates.Clear();
                GuildSettings.Clear();
                PrivateChannelSettings.Clear();
            }
        }

        internal void Dispose(bool destructor)
        {
            Logout();

            if (!destructor)
            {
                foreach (var stream in Livestreams.Values)
                    stream.Disconnect();

                foreach (var session in VoiceSessions.Values)
                    session.Disconnect();

                VoiceSessions.Clear();
                Livestreams.Clear();
                WebSocket.Dispose();

                Reset();
            }
        }


        public void Dispose()
        {
            Dispose(false);
        }
    }
}