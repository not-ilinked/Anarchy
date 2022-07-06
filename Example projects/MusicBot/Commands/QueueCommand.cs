﻿using Discord;
using Discord.Commands;

namespace MusicBot
{
    [Command("queue")]
    public class QueueCommand : CommandBase
    {
        [Parameter("action", true)]
        public string Action { get; private set; }

        public override void Execute()
        {
            if (Action == "clear")
            {
                if (Program.CanModifyList(Client, Message))
                {
                    var list = Program.TrackLists[Message.Guild.Id];

                    var currentSong = list.Tracks[0];
                    list.Tracks.Clear();

                    currentSong.CancellationTokenSource.Cancel();

                    Message.Channel.SendMessage("Queue has been cleared");
                }
            }
            else if (!Program.TrackLists.TryGetValue(Message.Guild.Id, out var list) || list.Tracks.Count == 0)
                Message.Channel.SendMessage("The music queue is empty");
            else
            {
                var embed = new EmbedMaker() { Title = "Current queue" };
                foreach (var song in list.Tracks)
                    embed.AddField(song.Title, song.ChannelName + (song == list.Tracks[0] ? " *(Currently playing)*" : ""));

                Message.Channel.SendMessage(embed);
            }
        }
    }
}
