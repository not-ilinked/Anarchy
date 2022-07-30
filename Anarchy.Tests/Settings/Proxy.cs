using System.Net;

namespace Discord.Settings
{
    internal class ProxySettings
    {
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; }

        public IWebProxy CreateProxy()
        {
            return new WebProxy(Host, Port);
        }
    }
}
