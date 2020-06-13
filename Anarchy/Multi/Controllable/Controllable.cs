using Discord.Gateway;
using System;

namespace Discord
{
    public abstract class Controllable : IDisposable
    {
        protected event EventHandler OnClientUpdated;

        private DiscordClient _client;
        public DiscordClient Client
        {
            get { return _client; }
            set
            {
                _client = value;

                OnClientUpdated?.Invoke(this, new EventArgs());
            }
        }


        internal bool SocketClient
        {
            get { return _client.GetType() == typeof(DiscordSocketClient); }
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}