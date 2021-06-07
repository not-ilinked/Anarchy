using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;
using Discord.Media;
using System.Threading;
using YoutubeExplode.Videos.Streams;
using System.IO;

namespace MusicBot
{
    [Command("play")]
    public class PlayCommand : CommandBase
    {
        private bool TryConnect()
        {
            var targetConnected = Client.GetVoiceStates(Message.Author.User.Id).GuildVoiceStates.TryGetValue(Message.Guild.Id, out var theirState);

            if (!targetConnected || theirState.Channel == null)
            {
                Message.Channel.SendMessage("You must be in a voice channel to play music");
                return false;
            }

            var channel = (VoiceChannel)Client.GetChannel(theirState.Channel.Id);

            if (!Program.ActiveSessions.TryGetValue(Message.Guild.Id, out var activeSession) || activeSession.Session.Channel.Id != theirState.Channel.Id)
            {
                var permissions = Client.GetCachedGuild(Message.Guild.Id).ClientMember.GetPermissions(channel.PermissionOverwrites);

                if (!permissions.Has(DiscordPermission.ConnectToVC) || !permissions.Has(DiscordPermission.SpeakInVC))
                {
                    Message.Channel.SendMessage("I lack permissions to play music in this channel");
                    return false;
                }

                else if (channel.UserLimit > 0 && Client.GetChannelVoiceStates(channel.Id).Count >= channel.UserLimit)
                {
                    Message.Channel.SendMessage("Your channel is full");
                    return false;
                }

                var session = Client.JoinVoiceChannel(new VoiceStateProperties() { ChannelId = channel.Id, Deafened = true, Muted = false });
                session.OnDisconnected += Session_OnDisconnected;
                session.Connect();

                // this should be using events, but i'm lazy lol
                while (session.State != MediaSessionState.Authenticated) Thread.Sleep(10);

                Program.ActiveSessions[Message.Guild.Id] = session.CreateStream(channel.Bitrate, AudioApplication.Music);
            }

            return true;
        }

        private void Session_OnDisconnected(DiscordVoiceSession session, DiscordMediaCloseEventArgs args)
        {
            Program.ActiveSessions.Remove(session.Guild.Id);
        }

        [Parameter("YouTube video URL")]
        public string Url { get; private set; }

        public override void Execute()
        {
            const string YouTubeVideo = "https://www.youtube.com/watch?v=";

            bool startTask = !Program.ActiveSessions.ContainsKey(Message.Guild.Id);
            if (TryConnect())
            {
                if (Url.StartsWith(YouTubeVideo))
                {
                    string id = Url.Substring(Url.IndexOf(YouTubeVideo) + YouTubeVideo.Length); // lazy

                    if (!Program.TrackLists.TryGetValue(Message.Guild.Id, out var list)) list = Program.TrackLists[Message.Guild.Id] = new List<AudioTrack>();
                    var track = new AudioTrack(id);
                    list.Add(track);

                    if (startTask)
                    {
                        Task.Run(() =>
                        {
                            Stream existingStream = null;

                            while (list.Count > 0)
                            {
                                // slightly scuffed
                                while (true) 
                                {
                                    if (!Program.ActiveSessions.TryGetValue(Message.Guild.Id, out var voiceStream)) return;
                                    else if (voiceStream.Session.State == MediaSessionState.Authenticated) break;
                                    else Thread.Sleep(10); 
                                }

                                var currentSong = list[0];

                                var manifest = Program.YouTubeClient.Videos.Streams.GetManifestAsync(currentSong.Id).Result;

                                if (existingStream == null)
                                {
                                    VoiceChannel currentChannel = (VoiceChannel)Client.GetChannel(Program.ActiveSessions[Message.Guild.Id].Session.Channel.Id);
                                    existingStream = DiscordVoiceUtils.GetAudioStream(GetVideoUrl(currentSong.Id, currentChannel.Bitrate));
                                }

                                if (Program.ActiveSessions[Message.Guild.Id].CopyFrom(existingStream, currentSong.CancellationTokenSource.Token))
                                {
                                    existingStream = null;
                                    list.RemoveAt(0);
                                }
                                else if (currentSong.CancellationTokenSource.IsCancellationRequested) existingStream = null;
                            }
                        });
                    }

                    Message.Channel.SendMessage($"Song \"{track.Title}\" has been added to the queue");
                }
                else Message.Channel.SendMessage("Please enter a valid YouTube video URL");
            }
        }

        private string GetVideoUrl(string videoId, uint channelBitrate)
        {
            var manifest = Program.YouTubeClient.Videos.Streams.GetManifestAsync(videoId).Result;

            AudioOnlyStreamInfo bestStream = null;
            foreach (var stream in manifest.GetAudioOnlyStreams().OrderBy(s => s.Bitrate))
            {
                if (bestStream == null || stream.Bitrate > bestStream.Bitrate)
                {
                    bestStream = stream;

                    if (stream.Bitrate.BitsPerSecond > channelBitrate)
                        break;
                }
            }

            return bestStream.Url;
        }
    }
}
