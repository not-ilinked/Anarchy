using System;

namespace Discord
{
    internal static class UserStatusConverter
    {
        public static string ToString(UserStatus status)
        {
            if (status == UserStatus.DoNotDisturb)
                return "dnd";
            else
                return status.ToString(); 
        }

        public static UserStatus FromString(string status)
        {
            if (status == null)
                return UserStatus.Offline;
            else if (status == "dnd")
                return UserStatus.DoNotDisturb;
            else
                return (UserStatus)Enum.Parse(typeof(UserStatus), status, true);
        }
    }
}
