namespace Discord
{
    public class DiscordEditablePermissions : DiscordPermissions
    {
        public DiscordEditablePermissions() : base() { }
        public DiscordEditablePermissions(uint permissions) : base(permissions) { }


        public void Add(DiscordPermission permission)
        {
            _value = DiscordPermissionCalculator.Add(_value, permission);
        }


        public void Remove(DiscordPermission permission)
        {
            _value = DiscordPermissionCalculator.Remove(_value, permission);
        }


        public static DiscordEditablePermissions operator+(DiscordEditablePermissions instance, DiscordPermission permission)
        {
            instance.Add(permission);
            return instance;
        }

        
        public static DiscordEditablePermissions operator-(DiscordEditablePermissions instance, DiscordPermission permission)
        {
            instance.Remove(permission);
            return instance;
        }
    }
}
