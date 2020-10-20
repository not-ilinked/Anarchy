using Discord;
using Discord.Commands;
using Discord.Gateway;
using System;

namespace MusicBot
{
    [Command("play", "Plays a song")]
    public class PlayCommand : CommandBase
    {
        // i'll add support for things like the bot looking up songs by the query later
        [Parameter("query")]
        public string Query { get; private set; }

        private ulong? GetStateChannelId(ulong userId)
        {
            if (Client.GetVoiceStates(userId).GuildVoiceStates.TryGetValue(Message.Guild.Id, out DiscordVoiceState state))
                return state.Channel == null ? null : (ulong?)state.Channel.Id;
            else
                return null;
        }

        public override void Execute()
        {
            if (Message.Guild != null)
            {
                var us = GetStateChannelId(Client.User.Id);

                if (us != null)
                {
                    // this would return true if both are null, but we've already checked for that
                    if (us == GetStateChannelId(Message.Author.User.Id))
                    {
                        var track = new MusicTrack(Query);
                        track.StartDownload();
                        Program.Players[Message.Guild.Id].Tracks.Add(track);

                        Message.Channel.SendMessage(new EmbedMaker()
                        {
                            Title = "Added track",
                            TitleUrl = track.Video.Url,
                            Description = $"Added \"{track.Video.Title}\" to the queue",
                            Color = Program.EmbedColor
                        });
                    }
                    else
                        Message.Channel.SendMessage("You must be in the same voice channel as me");
                }
                else
                    Message.Channel.SendMessage("I'm not connected to a voice channel in this server. Please use the join command");
            }
        }

        public override void HandleError(string parameterName, string providedValue, Exception exception)
        {
            if (providedValue == null)
                Message.Channel.SendMessage("No value provided for " + parameterName);
        }
    }
}
