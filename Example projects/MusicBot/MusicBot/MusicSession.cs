using Discord;
using Discord.Voice;
using System.Collections.Generic;
using System.Threading;

namespace MusicBot
{
    class Track
    {
        public Track(string name, string url, string file)
        {
            Name = name;
            Url = url;
            File = file;
        }

        public string Name { get; private set; }
        public string Url { get; private set; }
        public string File { get; private set; }
    }


    class MusicSession
    {
        public MusicSession(ulong guildId)
        {
            _loopQueue = new List<Track>();
            _guildId = guildId;
        }

        private ulong _guildId;
        public DiscordVoiceSession Session { get; set; }
        public VoiceChannel Channel { get; set; }
        public Queue<Track> Queue { get; set; }
        private List<Track> _loopQueue { get; set; }
        public Track CurrentTrack { get; set; }
        public bool Loop { get; set; }
        public bool SwitchingChannels { get; set; }
        public bool Paused { get; set; }
        private bool _stop;

        public void StartQueue()
        {
            while (true)
            {
                if (_stop)
                    return;

                try
                {
                    var track = this.Queue.Dequeue();

                    CurrentTrack = track;

                    byte[] file = DiscordVoiceUtils.ReadFromFile(track.File);
                    int offset = 0;
                    DiscordVoiceStream stream = Session.CreateStream(64 * 1024, AudioApplication.Music);

                    while (offset < file.Length)
                    {
                        if (Paused)
                        {
                            Thread.Sleep(100);
                        }
                        else
                        {
                            try
                            {
                                offset = stream.Write(file, offset);
                            }
                            catch
                            {
                                if (SwitchingChannels)
                                    Thread.Sleep(100);
                                else
                                    return;
                            }
                        }
                    }

                    if (Loop)
                        _loopQueue.Add(track);
                }
                catch
                {
                    if (Loop && _loopQueue.Count > 0)
                        this.Queue = new Queue<Track>(_loopQueue);
                    else
                        Thread.Sleep(100);
                }
            }
        }

        public void Disconnect()
        {
            Session.Disconnect();

            Program.Sessions.Remove(_guildId);

            _stop = true;
        }
    }
}
