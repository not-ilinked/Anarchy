using Newtonsoft.Json;

namespace Discord.Gateway
{
    public class VoiceStateProperties
    {
        internal readonly DiscordParameter<ulong?> GuildProperty = new DiscordParameter<ulong?>();
        [JsonProperty("guild_id")]
        public ulong? GuildId
        {
            get { return GuildProperty; }
            set { GuildProperty.Value = value; }
        }


        internal readonly DiscordParameter<ulong?> ChannelProperty = new DiscordParameter<ulong?>();
        [JsonProperty("channel_id")]
        public ulong? ChannelId 
        {
            get { return ChannelProperty; }
            set { ChannelProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> MutedProperty = new DiscordParameter<bool>();
        [JsonProperty("self_mute")]
        public bool Muted
        {
            get { return MutedProperty; }
            set { MutedProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> DeafProperty = new DiscordParameter<bool>();
        [JsonProperty("self_deaf")]
        public bool Deafened
        {
            get { return DeafProperty; }
            set { DeafProperty.Value = value; }
        }


        internal readonly DiscordParameter<bool> VideoProperty = new DiscordParameter<bool>();
        [JsonProperty("self_video")]
        public bool Video
        {
            get { return VideoProperty; }
            set { VideoProperty.Value = value; }
        }

        internal VoiceStateProperties Fill(DiscordSocketClient client)
        {
            if (client.Config.Cache)
            {
                DiscordVoiceStateContainer states;

                try
                {
                    states = client.GetVoiceStates(client.User.Id);
                }
                catch (DiscordHttpException ex)
                {
                    if (ex.Code == DiscordError.UnknownUser)
                        states = new DiscordVoiceStateContainer(client.User.Id);
                    else
                        throw;
                }

                DiscordVoiceState state = null;

                if (GuildProperty.Set)
                {
                    if (GuildId.HasValue)
                        states.GuildVoiceStates.TryGetValue(GuildId.Value, out state);
                    else
                        state = states.PrivateChannelVoiceState;

                    if (state != null && !ChannelProperty.Set)
                        ChannelId = state.Channel == null ? null : (ulong?)state.Channel.Id;
                }
                else if (ChannelProperty.Set && ChannelId.HasValue)
                {
                    var channel = client.GetChannel(ChannelId.Value);

                    if (channel.Type != ChannelType.Group && channel.Type != ChannelType.DM)
                    {
                        GuildId = ((GuildChannel)channel).GuildId;

                        if (states.GuildVoiceStates.TryGetValue(GuildId.Value, out DiscordVoiceState oldState))
                            state = oldState;
                    }
                    else if (states.PrivateChannelVoiceState != null && states.PrivateChannelVoiceState.Channel != null && states.PrivateChannelVoiceState.Channel.Id == ChannelId)
                        state = states.PrivateChannelVoiceState;
                }

                if (state != null)
                {
                    if (!DeafProperty.Set)
                        Deafened = state.SelfDeafened;

                    if (!MutedProperty.Set)
                        Muted = state.Muted;

                    if (!VideoProperty.Set)
                        Video = state.Video;
                }
            }

            return this;
        }
    }
}
