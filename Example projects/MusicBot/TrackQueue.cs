﻿using Discord;
using Discord.Gateway;
using Discord.Media;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Videos.Streams;

namespace MusicBot
{
    public class TrackQueue
    {
        public List<AudioTrack> Tracks { get; private set; }
        public bool Running { get; private set; }

        private readonly DiscordSocketClient _client;
        private readonly ulong _guildId;
        private Stream _stream;

        public TrackQueue(DiscordSocketClient client, ulong guildId)
        {
            _client = client;
            _guildId = guildId;
            Tracks = new List<AudioTrack>();
        }

        public void Start()
        {
            Running = true;

            Task.Run(() =>
            {
                DiscordVoiceClient voiceClient = _client.GetVoiceClient(_guildId);

                while (voiceClient.State == MediaConnectionState.Ready && Tracks.Count > 0)
                {
                    AudioTrack currentSong = Tracks[0];

                    StreamManifest manifest = Program.YouTubeClient.Videos.Streams.GetManifestAsync(currentSong.Id).Result;

                    if (_stream == null)
                    {
                        VoiceChannel currentChannel = (VoiceChannel)_client.GetChannel(voiceClient.Channel.Id);
                        _stream = DiscordVoiceUtils.GetAudioStream(GetVideoUrl(currentSong.Id, currentChannel.Bitrate));
                    }

                    if (voiceClient.Microphone.CopyFrom(_stream, currentSong.CancellationTokenSource.Token))
                    {
                        _stream = null;
                        Tracks.RemoveAt(0);
                    }
                    else if (currentSong.CancellationTokenSource.IsCancellationRequested)
                    {
                        _stream = null;
                    }
                }

                Running = false;
            });
        }

        private string GetVideoUrl(string videoId, uint channelBitrate)
        {
            StreamManifest manifest = Program.YouTubeClient.Videos.Streams.GetManifestAsync(videoId).Result;

            AudioOnlyStreamInfo bestStream = null;
            foreach (AudioOnlyStreamInfo stream in manifest.GetAudioOnlyStreams().OrderBy(s => s.Bitrate))
            {
                if (bestStream == null || stream.Bitrate > bestStream.Bitrate)
                {
                    bestStream = stream;

                    if (stream.Bitrate.BitsPerSecond > channelBitrate)
                    {
                        break;
                    }
                }
            }

            return bestStream.Url;
        }
    }
}
