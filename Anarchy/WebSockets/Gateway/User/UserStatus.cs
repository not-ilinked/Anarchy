using Newtonsoft.Json;
using System;

namespace Discord
{
    internal class UserStatusConverter : JsonConverter
    {
        private string ToString(UserStatus status)
        {
            if (status == UserStatus.DoNotDisturb)
            {
                return "dnd";
            }
            else
            {
                return status.ToString().ToLower();
            }
        }

        private UserStatus FromString(string status)
        {
            if (status == null)
            {
                return UserStatus.Offline;
            }
            else if (status == "dnd")
            {
                return UserStatus.DoNotDisturb;
            }
            else
            {
                return (UserStatus)Enum.Parse(typeof(UserStatus), status, true);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ToString((UserStatus)value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return FromString(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

    [JsonConverter(typeof(UserStatusConverter))]
    public enum UserStatus
    {
        Online,
        Idle,
        DoNotDisturb,
        Invisible,
        Offline
    }
}