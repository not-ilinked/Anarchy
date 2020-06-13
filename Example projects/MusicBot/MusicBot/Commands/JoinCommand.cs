using Discord;
using Discord.Commands;
using Discord.Gateway;
using Discord.Voice;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicBot
{
    [Command("join", "Makes the bot join the voice channel you're currently in")]
    public class JoinCommand : ICommand
    {
        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            VoiceChannel channel;

            try
            {
                channel = client.GetChannel(client.GetGuildVoiceStates(message.Guild).First(s => s.UserId == message.Author.User.Id).Channel.Id).ToVoiceChannel();
            }
            catch
            {
                message.Channel.SendMessage("You must be connected to a voice channel to play music");

                return;
            }

            if (!Program.Sessions.ContainsKey(message.Guild.Id))
            {
                DiscordVoiceSession voiceSession = client.JoinVoiceChannel(message.Guild, channel, false, true);

                voiceSession.OnConnected += (c, e) =>
                {
                    var session = new MusicSession(message.Guild)
                    {
                        Session = voiceSession,
                        Channel = channel,
                        Queue = new Queue<Track>(),
                    };

                    Program.Sessions.Add(message.Guild.Id, session);

                    message.Channel.SendMessage("Connected to voice channel.");

                    Task.Run(() => session.StartQueue());
                };
            }
            else if (Program.Sessions[message.Guild.Id].Channel.Id != channel.Id)
            {
                var session = Program.Sessions[message.Guild.Id];

                session.SwitchingChannels = true;
                session.Channel = channel;

                session.Session = client.JoinVoiceChannel(message.Guild, channel, false, true);
                session.Session.OnConnected += (sender, e) =>
                {
                    Program.Sessions[message.Guild.Id].SwitchingChannels = false;
                };
            }
        }
    }
}
