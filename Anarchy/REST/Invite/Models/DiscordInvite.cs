using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Discord
{
    public class DiscordInvite : ControllableEx
    {
        public DiscordInvite()
        {
            OnClientUpdated += (sender, e) =>
            {
                Inviter.SetClient(Client);

                if (Guild != null)
                    Guild.SetClient(Client);

                Channel.SetClient(Client);
            };
            JsonUpdated += (sender, json) => Channel.SetJson(json.Value<JObject>("channel"));
        }


        [JsonProperty("code")]
        public string Code { get; private set; }


        [JsonProperty("channel")]
        public DiscordChannel Channel { get; private set; }


        [JsonProperty("inviter")]
        public DiscordUser Inviter { get; private set; }


        [JsonProperty("guild")]
        public MinimalGuild Guild { get; private set; }


        public InviteType Type
        {
            get { return Guild != null ? InviteType.Guild : InviteType.Group; }
        }


        /// <summary>
        /// Deletes the invite
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            Inviter = Client.DeleteInvite(Code).Inviter;
        }


        public void Join()
        {
            if (Type == InviteType.Guild)
                Client.JoinGuild(Code);
            else
                Client.JoinGroup(Code);
        }
        

        public override string ToString()
        {
            return Code;
        }


        public static implicit operator string(DiscordInvite instance)
        {
            return instance.Code;
        }


        public GuildInvite ToGuildInvite()
        {
            if (Type != InviteType.Guild)
                throw new InvalidConvertionException(Client, "Invite is not of a guild");

            return Json.ToObject<GuildInvite>().SetClient(Client).SetJson(Json);
        }
    }
}
