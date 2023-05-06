using System.Text.Json;
using System;
using System.Text.Json.Serialization;

namespace Discord
{
    internal class UserStatusConverter : JsonConverter<UserStatus>
    {
        private string ToString(UserStatus status)
        {
            if (status == UserStatus.DoNotDisturb)
                return "dnd";
            else
                return status.ToString().ToLower();
        }

        private UserStatus FromString(string status)
        {
            if (status == null)
                return UserStatus.Offline;
            else if (status == "dnd")
                return UserStatus.DoNotDisturb;
            else
                return (UserStatus) Enum.Parse(typeof(UserStatus), status, true);
        }

        public override UserStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return FromString(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, UserStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ToString(value));
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