using System.Text;
using Microsoft.Maui.Graphics;

namespace Discord
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void SendMessage()
        {
            const string content = "The simplest possible message.";

            var msg = Globals.Client.SendMessage(
                Globals.Settings.ChannelId,
                content);

            Assert.AreEqual(content, msg.Content);
        }

        [TestMethod]
        public void SendFile()
        {
            const string content = $"{nameof(SendFile)} attachment test.";

            var msg = Globals.Client.SendFile(
                Globals.Settings.ChannelId,
                Globals.FileNames.PoetryTxt,
                content);

            Assert.AreEqual(1, msg.Attachments.Count);
            Assert.IsTrue(msg.Attachments[0].ContentType.StartsWith(MediaTypeNames.Text.Plain));
        }

        [TestMethod]
        public void SendMessagePropertiesWithAttachments()
        {
            var image1 = DiscordImageSource.FromStream(
                File.OpenRead(Globals.FileNames.Image1), ImageFormat.Png);
            var image2 = DiscordImageSource.FromStream(
                File.OpenRead(Globals.FileNames.Image2), ImageFormat.Jpeg);

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
                        new DiscordAttachmentFile(Encoding.UTF8.GetBytes(File.ReadAllText(Globals.FileNames.PoetryTxt))),
                        Globals.FileNames.PoetryTxt,
                        "Text File")
                }
            };

            var msg = Globals.Client.SendMessage(
                Globals.Settings.ChannelId,
                props);

            Assert.AreEqual(props.Attachments.Count, msg.Attachments.Count);

            var file = msg.Attachments[0].GetAttachmentFile().Result;
            var postedImage1 = file.IsImage() ? DiscordImageSource.FromFile(file) : null;
            Assert.IsNotNull(postedImage1);

            CollectionAssert.AreEqual(postedImage1.PlatformImage.Bytes, image1.PlatformImage.Bytes);
        }
    }
}