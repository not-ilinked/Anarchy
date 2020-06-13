using System.Collections.Generic;
using System.Linq;

namespace Discord
{
    public static class DiscordPermissionCalculator
    {
        /// <summary>
        /// Creates an <see cref="DiscordEditablePermissions"/> from scratch
        /// </summary>
        /// <param name="perms">The permissions to add</param>
        public static DiscordEditablePermissions Create(List<DiscordPermission> perms)
        {
            uint permissions = 512;
            perms = perms.Distinct().ToList();
            perms.ForEach(perm => permissions = Add(permissions, perm));
            return new DiscordEditablePermissions(permissions);
        }


        /// <summary>
        /// Adds a permission to an existing permission list
        /// </summary>
        /// <param name="permissions">Permissions to add to</param>
        /// <param name="permission">Permission to add</param>
        public static uint Add(uint permissions, DiscordPermission permission)
        {
            if (!Has(permissions, permission))
                permissions |= (uint)permission;

            return permissions;
        }


        /// <summary>
        /// Removes a permission from an existing permission list
        /// </summary>
        /// <param name="permissions">Permissions to remove from</param>
        /// <param name="permission">Permission to remove</param>
        public static uint Remove(uint permissions, DiscordPermission permission)
        {
            if (Has(permissions, permission))
                permissions ^= (uint)permission;

            return permissions;
        }


        /// <summary>
        /// Checks if a permission list has a permission or not
        /// </summary>
        /// <param name="permission">Permission to check</param>
        public static bool Has(uint permissions, DiscordPermission permission)
        {
            return (permissions & (uint)permission) == (uint)permission;
        }
    }
}
