using System.Collections.Generic;

namespace Discord
{
    public class VerificationFormProperties
    {
        private readonly DiscordParameter<string> _descParam = new DiscordParameter<string>();
        public string Description
        {
            get => _descParam;
            set => _descParam.Value = value;
        }

        public bool ShouldSerializeDescription()
        {
            return _descParam.Set;
        }

        public List<GuildVerificationFormField> Fields { get; set; }
        public bool ShouldSerializeFields()
        {
            return Fields != null;
        }

        private readonly DiscordParameter<bool> _enabledParam = new DiscordParameter<bool>();
        public bool Enabled
        {
            get => _enabledParam;
            set => _enabledParam.Value = value;
        }

        public bool ShouldSerializeEnabled()
        {
            return _enabledParam.Set;
        }
    }
}
