using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord.Gateway
{
    public class DiscordPresence : Controllable
    {
        public DiscordPresence()
        {
            OnClientUpdated += (sender, e) =>
            {
                Activities.SetClientsInList(Client);
            };
        }

        [JsonProperty("user")]
        private readonly JObject _user;

        public ulong UserId
        {
            get { return _user["id"].ToObject<ulong>(); }
        }

        private readonly DiscordParameter<List<DiscordActivity>> _activitiesParam = new DiscordParameter<List<DiscordActivity>>();
        [JsonProperty("activities")]
        [JsonConverter(typeof(DeepJsonConverter<DiscordActivity>))]
        private List<DiscordActivity> _activities
        {
            get { return _activitiesParam; }
            set { _activitiesParam.Value = value; }
        }

        public IReadOnlyList<DiscordActivity> Activities
        {
            get { return _activities; }
        }

        public bool ActivitiesSet
        {
            get { return _activitiesParam.Set; }
        }

        private readonly DiscordParameter<UserStatus> _statusParam = new DiscordParameter<UserStatus>();
        [JsonProperty("status")]
        public UserStatus Status
        {
            get { return _statusParam; }
            private set { _statusParam.Value = value; }
        }

        public bool StatusSet
        {
            get { return _statusParam.Set; }
        }

        private readonly DiscordParameter<ActiveSessionPlatforms> _platformsParam = new DiscordParameter<ActiveSessionPlatforms>();
        [JsonProperty("client_status")]
        public ActiveSessionPlatforms ActivePlatforms
        {
            get { return _platformsParam; }
            set { _platformsParam.Value = value; }
        }

        public bool ActivePlatformsSet
        {
            get { return _platformsParam.Set; }
        }

        internal void Update(DiscordPresence presence)
        {
            if (presence.ActivePlatformsSet)
                ActivePlatforms = presence.ActivePlatforms;
            if (presence.ActivitiesSet)
                _activities = presence.Activities.ToList();
            if (presence.StatusSet)
                Status = presence.Status;
        }

        public override string ToString()
        {
            return UserId.ToString();
        }
    }
}
