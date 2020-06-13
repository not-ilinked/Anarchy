namespace Discord
{
#pragma warning disable CS0660, CS0661
    public class DiscordPermissions
#pragma warning restore CS0660, CS0661
    {
        protected uint _value;

        public DiscordPermissions()
        {
            _value = 512;
        }


        public DiscordPermissions(uint permissions)
        {
            _value = permissions;
        }


        public bool Has(DiscordPermission permission)
        {
            return DiscordPermissionCalculator.Has(_value, permission);
        }


        public static implicit operator uint(DiscordPermissions instance)
        {
            return instance._value;
        }
    }
}