using System.Collections.Generic;

namespace Discord
{
    public static class ControllableExtensions
    {
        public static T SetClient<T>(this T @class, DiscordClient client) where T : Controllable
        {
            if (@class != null)
            {
                @class.Client = client;
            }

            return @class;
        }


        internal static IReadOnlyList<T> SetClientsInList<T>(this IReadOnlyList<T> classes, DiscordClient client) where T : Controllable
        {
            if (classes != null)
            {
                foreach (T @class in classes)
                {
                    @class.Client = client;
                }
            }
            return classes;
        }
    }
}