using Discord;
using Discord.Media;
using System.Collections.Generic;
using System.IO;
using Discord.Gateway;
using System.Threading.Tasks;

namespace MusicBot
{
    public class MusicPlayer
    {
        public DiscordVoiceSession Session { get; private set; }
        public DiscordVoiceStream SessionStream { get; private set; }
        public List<MusicTrack> Tracks { get; private set; }
        public Stream CurrentTrackStream { get; private set; }
        public DiscordVoiceStream CurrentStream { get; private set; }

        public MusicPlayer(DiscordVoiceSession session)
        {
            SetSession(session);
            Tracks = new List<MusicTrack>();
        }

        public void SetSession(DiscordVoiceSession newSession)
        {
            Session = newSession;
            SessionStream = Session.CreateStream(((VoiceChannel)newSession.Client.GetChannel(newSession.Channel.Id)).Bitrate, AudioApplication.Music);
        }

        public async Task StartAsync()
        {
            while (true)
            {
                if (Session.State == MediaSessionState.Authenticated && Tracks.Count > 0)
                {
                    MusicTrack currentTrack = Tracks[0];

                    if (currentTrack.DownloadTask != null)
                        await currentTrack.DownloadTask;

                    if (CurrentTrackStream == null)
                        CurrentTrackStream = DiscordVoiceUtils.GetAudioStream(currentTrack.FilePath);

                    try
                    {
                        SessionStream.CopyFrom(CurrentTrackStream);

                        Tracks.RemoveAt(0);
                        CurrentTrackStream.Flush();
                        CurrentTrackStream = null;
                    }
                    catch { }
                }
                else
                    await Task.Delay(100);
            }
        }
    }
}
