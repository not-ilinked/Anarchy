namespace Anarchy.Client.Rest;

using System.Text;
using Discord;
using Microsoft.Maui.Graphics;

[TestClass]
public class MessageTests
{
    [TestMethod]
    public void SendFile()
    {
        var c = Accounts.User.Clients.RestClient;
        var o = Accounts.User.Options;

        var msg = c.SendFile(
            o.ChannelId,
            Resources.Filenames.PoetryTxt,
            $"{nameof(SendFile)} attachment test.");

        Assert.AreEqual(1, msg.Attachments.Count);
    }

    [TestMethod]
    public void SendMessage()
    {
        var c = Accounts.User.Clients.RestClient;
        var o = Accounts.User.Options;

        const string content = "The simplest possible message.";

        var msg = c.SendMessage(o.ChannelId, content);

        Assert.AreEqual(content, msg.Content);
    }

    [TestMethod]
    public void SendMessageWithAttachments()
    {
        var image1 = DiscordImageSource.FromStream(File.OpenRead(Resources.Filenames.Image1), ImageFormat.Png);
        var image2 = DiscordImageSource.FromStream(File.OpenRead(Resources.Filenames.Image2), ImageFormat.Jpeg);
        var poetryText = File.ReadAllText(Resources.Filenames.PoetryTxt);

        var props = new MessageProperties
        {
            Content = $"{nameof(MessageProperties)} attachment test.",
            Attachments = new List<PartialDiscordAttachment>()
            {
                // Attach an arbitrary file.
                new PartialDiscordAttachment(Resources.Filenames.Image1),
                // Attach an existing DiscordImage.
                new PartialDiscordAttachment(image2, Resources.Filenames.Image2),
                // Attach an arbitrary file with custom specifications for everything.
                new PartialDiscordAttachment(
                    new DiscordAttachmentFile(Encoding.UTF8.GetBytes(poetryText), MediaTypeNames.Text.Plain),
                    Resources.Filenames.PoetryTxt,
                    "Text File")
            }
        };

        var c = Accounts.User.Clients.RestClient;
        var o = Accounts.User.Options;

        var msg = c.SendMessage(o.ChannelId, props);

        Assert.AreEqual(props.Attachments.Count, msg.Attachments.Count);

        var postedImage1 = DiscordImageSource.FromUrl(msg.Attachments[0].Url).Result;
        CollectionAssert.AreEqual(postedImage1.PlatformImage.Bytes, image1.PlatformImage.Bytes);
    }
}