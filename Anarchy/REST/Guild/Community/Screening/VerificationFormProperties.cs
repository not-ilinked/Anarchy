using System.Collections.Generic;

namespace Discord
{
    public class VerificationFormProperties
    {
        private readonly DiscordParameter<string> _descParam = new DiscordParameter<string>();
        public string Description
        {
            get { return _descParam; }
            set { _descParam.Value = value; }
        }

        public bool ShouldSerializeDescription() => _descParam.Set;


        public List<GuildVerificationFormField> Fields { get; set; }
        public bool ShouldSerializeFields() => Fields != null;


        private readonly DiscordParameter<bool> _enabledParam = new DiscordParameter<bool>();
        public bool Enabled
        {
            get { return _enabledParam; }
            set { _enabledParam.Value = value; }
        }

        public bool ShouldSerializeEnabled() => _enabledParam.Set;
    }
}
