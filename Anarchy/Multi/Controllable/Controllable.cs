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


        public void Dispose()
        {
            _client = null;
        }
    }
}