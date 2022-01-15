using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Discord
{
    public class LanguageConverter : JsonConverter
    {
        private static readonly Dictionary<DiscordLanguage, string> Languages = new Dictionary<DiscordLanguage, string>()
        {
            { DiscordLanguage.Danish, "da" },
            { DiscordLanguage.German, "de" },
            { DiscordLanguage.EnglishUK, "en-GB" },
            { DiscordLanguage.EnglishUS, "en-US" },
            { DiscordLanguage.Spanish, "es-ES" },
            { DiscordLanguage.French, "fr" },
            { DiscordLanguage.Croatian, "hr" },
            { DiscordLanguage.Italian, "it" },
            { DiscordLanguage.Hungarian, "hu" },
            { DiscordLanguage.Dutch, "nl" },
            { DiscordLanguage.Norwegian, "no" },
            { DiscordLanguage.Polish, "pl" },
            { DiscordLanguage.Portuguese, "pt-BR" },
            { DiscordLanguage.Romanian, "ro" },
            { DiscordLanguage.Finnish, "fi" },
            { DiscordLanguage.Swedish, "sv-SE" },
            { DiscordLanguage.Viatnamese, "vi" },
            { DiscordLanguage.Turkish, "tr" },
            { DiscordLanguage.Czech, "cs" },
            { DiscordLanguage.Greek, "el" },
            { DiscordLanguage.Russian, "ru" },
            { DiscordLanguage.Ukranian, "uk" },
            { DiscordLanguage.Thai, "th" },
            { DiscordLanguage.Chinese, "zh-CN" },
            { DiscordLanguage.Japanese, "ja" },
            { DiscordLanguage.Korean, "ko" },
            { DiscordLanguage.Bulgarian, "bg" },
            { DiscordLanguage.Taiwanese, "zh-TW" },
            { DiscordLanguage.Lithuanian, "lt" },
            { DiscordLanguage.Hindi, "hi" }
        };

        private string ToString(DiscordLanguage lang)
        {
            return Languages[lang];
        }

        private DiscordLanguage FromString(string langStr)
        {
            foreach (KeyValuePair<DiscordLanguage, string> language in Languages)
            {
                if (language.Value == langStr)
                {
                    return language.Key;
                }
            }

            throw new InvalidOperationException("Invalid language string");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(ToString((DiscordLanguage)value));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return FromString(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

    [JsonConverter(typeof(LanguageConverter))]
    public enum DiscordLanguage
    {
        Danish,
        German,
        EnglishUK,
        EnglishUS,
        Spanish,
        French,
        Croatian,
        Italian,
        Lithuanian,
        Hungarian,
        Dutch,
        Norwegian,
        Polish,
        Portuguese,
        Romanian,
        Finnish,
        Swedish,
        Viatnamese,
        Turkish,
        Czech,
        Greek,
        Bulgarian,
        Russian,
        Ukranian,
        Thai,
        Chinese,
        Japanese,
        Taiwanese,
        Korean,
        Hindi
    }
}
