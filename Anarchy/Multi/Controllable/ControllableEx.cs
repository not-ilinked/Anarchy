using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

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

                JsonUpdated?.Invoke(this, value);
            }
        }


        internal void UpdateSelfJson()
        {
            foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (var attr in field.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(JsonPropertyAttribute))
                    {
                        var value = field.GetValue(this);

                        Json[attr.ConstructorArguments[0].Value.ToString()] = value == null ? null : JProperty.FromObject(value);

                        break;
                    }
                }
            }


            foreach (var property in this.GetType().GetProperties())
            {
                foreach (var attr in property.CustomAttributes)
                {
                    if (attr.AttributeType == typeof(JsonPropertyAttribute))
                    {
                        var value = property.GetValue(this);

                        Json[attr.ConstructorArguments[0].Value.ToString()] = value == null ? null : JProperty.FromObject(property.GetValue(this));

                        break;
                    }
                }
            }

            JsonUpdated?.Invoke(this, Json);
        }


        public new void Dispose()
        {
            base.Dispose();
            _json = null;
        }
    }
}
