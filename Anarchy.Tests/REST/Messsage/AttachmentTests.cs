using System.Text;
using Microsoft.Maui.Graphics;

namespace Discord
{
    [TestClass]
    public class AttachmentTests
    {
        [TestMethod]
        public void SendFile()
        {
            var client = Globals.Client;
            var channelId = Globals.Settings.ChannelId;

            var msg = client.SendFile(channelId, Globals.FileNames.PoetryTxt, $"{nameof(SendFile)} attachment test.");

            Assert.AreEqual(1, msg.Attachments.Count);
        }

        [TestMethod]
        public void SendMessageProperties()
        {
            var image1 = DiscordImageSource.FromStream(File.OpenRead(Globals.FileNames.Image1), ImageFormat.Png);
            var image2 = DiscordImageSource.FromStream(File.OpenRead(Globals.FileNames.Image2), ImageFormat.Jpeg);
            var poetryText = File.ReadAllText(Globals.FileNames.PoetryTxt);

            var props = new MessageProperties
            {
                Content = $"{nameof(MessageProperties)} attachment test.",
                Attachments = new List<PartialDiscordAttachment>()
                {
                    // Attach an arbitrary file.
                    new PartialDiscordAttachment(Globals.FileNames.Image1),
                    // Attach an existing DiscordImage.
                    new PartialDiscordAttachment(image2, Globals.FileNames.Image2),
                    // Attach an arbitrary file with custom specifications for everything.
                    new PartialDiscordAttachment(
                        new DiscordAttachmentFile(Encoding.UTF8.GetBytes(poetryText), MediaTypeNames.Text.Plain),
                        Globals.FileNames.PoetryTxt,
                        "Text File")
                }
            };

            var client = Globals.Client;
            var channelId = Globals.Settings.ChannelId;

            var msg = client.SendMessage(channelId, props);
            Assert.AreEqual(props.Attachments.Count, msg.Attachments.Count);

            var postedImage1 = DiscordImageSource.FromUrl(msg.Attachments[0].Url).Result;
            CollectionAssert.AreEqual(postedImage1.PlatformImage.Bytes, image1.PlatformImage.Bytes);
        }
    }
}