using Discord;
using Discord.Commands;
using Discord.Gateway;
using Discord.Media;

namespace MusicBot
{
    [Command("play")]
    public class PlayCommand : CommandBase
    {
        [Parameter("YouTube video URL")]
        public string Url { get; private set; }

        public override void Execute()
        {
            const string YouTubeVideo = "https://www.youtube.com/watch?v=";

            var targetConnected = Client.GetVoiceStates(Message.Author.User.Id).GuildVoiceStates.TryGetValue(Message.Guild.Id, out var theirState);

            if (!targetConnected || theirState.Channel == null)
            {
                Message.Channel.SendMessage("You must be in a voice channel to play music");
                return;
            }

            var channel = (VoiceChannel) Client.GetChannel(theirState.Channel.Id);
            var voiceClient = Client.GetVoiceClient(Message.Guild.Id);

            if (voiceClient.State < MediaConnectionState.Ready || voiceClient.Channel.Id != channel.Id)
            {
                var permissions = Client.GetCachedGuild(Message.Guild.Id).ClientMember.GetPermissions(channel.PermissionOverwrites);

                if (!permissions.Has(DiscordPermission.ConnectToVC) || !permissions.Has(DiscordPermission.SpeakInVC))
                {
                    Message.Channel.SendMessage("I lack permissions to play music in this channel");
                    return;
                }

                else if (channel.UserLimit > 0 && Client.GetChannelVoiceStates(channel.Id).Count >= channel.UserLimit)
                {
                    Message.Channel.SendMessage("Your channel is full");
                    return;
                }
            }

            if (Url.StartsWith(YouTubeVideo))
            {
                string id = Url.Substring(Url.IndexOf(YouTubeVideo) + YouTubeVideo.Length); // lazy

                var track = new AudioTrack(id);
                if (!Program.TrackLists.TryGetValue(Message.Guild.Id, out var list)) list = Program.TrackLists[Message.Guild.Id] = new TrackQueue(Client, Message.Guild.Id);
                list.Tracks.Add(track);

                Message.Channel.SendMessage($"Song \"{track.Title}\" has been added to the queue");

                if (voiceClient.State < MediaConnectionState.Ready || voiceClient.Channel.Id != channel.Id)
                    voiceClient.Connect(channel.Id, new VoiceConnectionProperties() { Deafened = true });
                else if (!list.Running)
                    list.Start();
            }
            else Message.Channel.SendMessage("Please enter a valid YouTube video URL");
        }
    }
}
