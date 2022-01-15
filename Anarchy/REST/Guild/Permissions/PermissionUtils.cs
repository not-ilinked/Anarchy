using System;

namespace Discord
{
    public static class PermissionUtils
    {
        public static DiscordPermission GetAllPermissions()
        {
            DiscordPermission permissions = DiscordPermission.None;
            foreach (DiscordPermission permission in Enum.GetValues(typeof(DiscordPermission)))
            {
                permissions |= permission;
            }

            return permissions;
        }

        public static DiscordPermission Add(this DiscordPermission permissions, DiscordPermission newPerms)
        {
            return permissions | newPerms;
        }

        private static bool And(DiscordPermission flags, DiscordPermission other)
        {
            return (flags & other) == other;
        }

        public static bool Has(this DiscordPermission permissions, DiscordPermission requiredPermission)
        {
            if (And(permissions, DiscordPermission.Administrator))
            {
                return true;
            }
            else
            {
                return And(permissions, requiredPermission);
            }
        }

        public static DiscordPermission Remove(this DiscordPermission permissions, DiscordPermission permsToRemove)
        {
            return permissions & ~permsToRemove;
        }
    }
}
