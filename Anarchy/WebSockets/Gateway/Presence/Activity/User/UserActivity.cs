using Newtonsoft.Json;
using System;

namespace Discord.Gateway
{
    public class UserActivity : ControllableEx
    {
        [JsonProperty("type")]
        public ActivityType Type { get; private set; }


        [JsonProperty("id")]
        public string Id { get; private set; }


        [JsonProperty("name")]
        public string Name { get; private set; }


        public UserGameActivity ToGameActivity()
        {
            if (Type != ActivityType.Game)
                throw new InvalidOperationException("Activity is not a game");

            return Json.ToObject<UserGameActivity>().SetJson(Json);
        }


        public UserListeningActivity ToListeningActivity()
        {
            if (Type != ActivityType.Listening)
                throw new InvalidOperationException("Activity is not of type Listening");

            return Json.ToObject<UserListeningActivity>().SetJson(Json);
        }


        public UserCustomStatusActivity ToCustomStatusActivity()
        {
            if (Type != ActivityType.CustomStatus)
                throw new InvalidOperationException("Activity is not a custom status");

            return Json.ToObject<UserCustomStatusActivity>().SetJson(Json);
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
