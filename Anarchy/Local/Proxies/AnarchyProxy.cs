using Leaf.xNet;
using System;

namespace Discord
{
    public class AnarchyProxy
    {
        public AnarchyProxyType Type { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public void Parse(string proxy)
        {
            string[] split = proxy.Split(':');

            Host = split[0];
            Port = int.Parse(split[1]);

            if (split.Length == 4)
            {
                Username = split[2];
                Password = split[3];
            }
        }

        public ProxyClient CreateProxyClient()
        {
            switch (Type)
            {
                case AnarchyProxyType.HTTP:
                    return new HttpProxyClient(Host, Port, Username, Password);
                case AnarchyProxyType.Socks4:
                    return new Socks4ProxyClient(Host, Port, Username);
                case AnarchyProxyType.Socks4a:
                    return new Socks4AProxyClient(Host, Port, Username);
                case AnarchyProxyType.Socks5:
                    return new Socks5ProxyClient(Host, Port, Username, Password);
                default:
                    throw new ArgumentException("Invalid Type");
            }
        }
    }
}
