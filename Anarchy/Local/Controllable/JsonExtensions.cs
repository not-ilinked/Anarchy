using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Discord
{
    internal static class JsonExtensions
    {
        internal static bool TryFindTypes(Type type, out Dictionary<int, Type> types)
        {
            if (DeepJsonConverter.RecognizedTypes.TryGetValue(type, out types))
                return true;
            else if (type.BaseType == null)
            {
                types = null;
                return false;
            }
            else
                return TryFindTypes(type.BaseType, out types);
        }

        public static T ParseDeterministic<T>(this JsonElement element, JsonSerializerOptions options)
        {
            if (TryFindTypes(typeof(T), out Dictionary<int, Type> types))
            {
                int type = element.GetProperty("type").GetInt32();
                return JsonSerializer.Deserialize<T>(element.GetRawText(), types.TryGetValue(type, out var t) ? options.Converters.Add(new DeepJsonConverter(t)) : options);
            }
            else
                throw new InvalidCastException("Unable to find any implementations for T");
        }

        public static List<T> MultipleDeterministic<T>(this JsonElement element, JsonSerializerOptions options)
        {
            List<T> results = new List<T>();

            foreach (JsonElement child in element.EnumerateArray())
                results.Add(child.ParseDeterministic<T>(options));

            return results;
        }
    }
}