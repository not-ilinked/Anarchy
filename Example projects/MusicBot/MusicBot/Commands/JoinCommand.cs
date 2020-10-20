using Discord.Commands;
using Discord.Gateway;
using Discord.Media;
using System;

namespace MusicBot
{
    [Command("join", "Joins the voice channel you're in")]
    public class JoinCommand : CommandBase
    {
        public override void Execute()
        {
            if (Message.Guild != null)
            {
                if (Client.GetVoiceStates(Message.Author.User.Id).GuildVoiceStates.TryGetValue(Message.Guild.Id, out DiscordVoiceState state) && state.Channel != null)
                {
                    var session = Client.JoinVoiceChannel(new VoiceStateProperties() { ChannelId = state.Channel.Id });
                    session.ReceivePackets = false;
                    session.OnConnected += Session_OnConnected;
                    session.Connect();
                }
                else
                    Message.Channel.SendMessage("You must be connected to a voice channel to use this command");
            }
        }

        private async void Session_OnConnected(DiscordVoiceSession session, EventArgs e)
        {
            Message.Channel.SendMessage("Connected");

            if (Program.Players.TryGetValue(Message.Guild.Id, out MusicPlayer player))
                player.SetSession(session);
            else
            {
                player = Program.Players[Message.Guild.Id] = new MusicPlayer(session);
                await player.StartAsync();
            }
        }
    }
}
