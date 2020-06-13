using Discord;
using Discord.Commands;
using Discord.Gateway;
using System;
using DotNetTools.SharpGrabber.Internal.Grabbers;
using System.IO;
using System.Net.Http;

namespace MusicBot
{
    [Command("play", "Adds a track to the queue")]
    public class PlayCommand : ICommand
    {
        [Parameter("YouTube song url")]
        public string Url { get; private set; }

        public void Execute(DiscordSocketClient client, DiscordMessage message)
        {
            if (!Program.Sessions.ContainsKey(message.Guild))
                message.Channel.SendMessage("Not connected to a voice channel. Use the join command to play music.");
            else
            {
                if (Url.IndexOf("?v=") > -1)
                {
                    string videoId = Url.Substring(Url.IndexOf("?v=") + 3, 11);

                    string basePath = $"Cache/{videoId}";
                    string path = basePath + ".webm";

                    string videoName = null;

                    if (!File.Exists(path))
                    {
                        try
                        {
                            var result = new YouTubeGrabber().GrabAsync(new Uri(Url)).Result;

                            foreach (var resource in result.Resources)
                            {
                                if (resource.ResourceUri.Host.Contains("googlevideo.com") && resource.ResourceUri.Query.Contains("mime=audio"))
                                {
                                    File.WriteAllBytes(path, new HttpClient().GetAsync(resource.ResourceUri.ToString()).Result.Content.ReadAsByteArrayAsync().Result);
                                    File.WriteAllText(basePath + ".txt", result.Title);

                                    videoName = result.Title;

                                    break;
                                }
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                    else
                        videoName = File.ReadAllText(basePath + ".txt");

                    message.Channel.SendMessage($"Added \"{videoName}\" to queue.");

                    Program.Sessions[message.Guild].Queue.Enqueue(new Track(videoName, Url, path));
                }
                else
                    message.Channel.SendMessage($"That appears to not be a valid YouTube video url, <@{message.Author.User.Id}>");
            }
        }
    }
}
