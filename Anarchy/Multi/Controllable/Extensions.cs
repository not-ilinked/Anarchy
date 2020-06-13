using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Discord
{
    public static class ControllableExtensions
    {
        public static T SetClient<T>(this T @class, DiscordClient client) where T : Controllable
        {
            if (@class != null)
                @class.Client = client;
            return @class;
        }


        public static IReadOnlyList<T> SetClientsInList<T>(this IReadOnlyList<T> classes, DiscordClient client) where T : Controllable
        {
            if (classes != null)
            {
                foreach (var @class in classes)
                    @class.Client = client;
            }
            return classes;
        }


        internal static T SetJson<T>(this T @class, JObject obj) where T : ControllableEx
        {
            @class.Json = obj;
            return @class;
        }
    }
}