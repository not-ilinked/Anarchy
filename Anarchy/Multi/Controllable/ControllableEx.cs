using Newtonsoft.Json.Linq;
using System;
using System.Linq;

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


        internal void UpdateSelfJson()
        {
            JObject updated = JObject.FromObject(this);

            foreach (var property in updated.Properties())
            {
                if (!property.Name.Any(char.IsUpper)) // json.net has a habit of serializing properties without JsonProperty attributes
                    Json[property.Name] = property.Value;
            }

            JsonUpdated?.Invoke(this, Json);
            //SignalClientUpdate(); // currently we completely overwrite lists when JsonUpdated is fired, causing ControllableEx Client references to dissapear. this is a temporary fix for that issue
        }


        public new void Dispose()
        {
            base.Dispose();
            _json = null;
        }
    }
}
