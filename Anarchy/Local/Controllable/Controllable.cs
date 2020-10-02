using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public abstract class Controllable : IDisposable
    {
        protected event EventHandler OnClientUpdated;

        private DiscordClient _client;
        [JsonIgnore]
        internal DiscordClient Client
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