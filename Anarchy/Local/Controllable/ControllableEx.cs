using Newtonsoft.Json.Linq;
using System;

namespace Discord
{
    public class ControllableEx : Controllable, IDisposable
    {
        internal delegate void JsonHandler(object sender, JObject json);
        internal event JsonHandler JsonUpdated;

        private JObject _json;
        internal JObject Json
        {
            get
            {
                return _json;
            }
            set 
            {
                _json = value;

                JsonUpdated?.Invoke(this, _json);
            }
        }


        public new void Dispose()
        {
            base.Dispose();
            _json = null;
        }
    }
}
