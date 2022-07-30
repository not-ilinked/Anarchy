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
    }
}
