using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Model
{
    public class Language
    {
        public string Name { get; set; }
        public string NetLanguageName { get; set; }
        public string CountryMobilePrefix { get; set; }

        public string Flag { get; set; }

        public bool Selected { get; set; }

        public static List<Language> SupportedLanguages => new List<Language>
    {
            new Language{ Name = "English",NetLanguageName=Codes.English,CountryMobilePrefix = "+44",Flag="flag_usa.png"},
            new Language{ Name = "Ελληνικά",NetLanguageName=Codes.Greek,CountryMobilePrefix = "+30",Flag="flag_greece.png"},
            new Language{ Name = "French",NetLanguageName=Codes.French,CountryMobilePrefix = "+33",Flag="flag_france.png"},
            new Language{ Name = "German",NetLanguageName=Codes.German,CountryMobilePrefix = "+49",Flag="flag_germany.png"},
            new Language{ Name = "Italian",NetLanguageName=Codes.Italian,CountryMobilePrefix = "+39"},
            new Language{ Name = "Russian",NetLanguageName=Codes.Russian,CountryMobilePrefix = "+7"},
            new Language{ Name = "Bulgarian",NetLanguageName=Codes.Bulgarian,CountryMobilePrefix = "+359"},
            new Language{ Name = "Chinese",NetLanguageName=Codes.Chinese,CountryMobilePrefix = "+86"}
    };

        public static class Codes
        {
            public const string English = "en-US";
            public const string Greek = "el-GR";
            public const string French = "fr-FR";
            public const string German = "de-DE";
            public const string Italian = "it-IT";
            public const string Russian = "ru-RU";
            public const string Bulgarian = "bg-BG";
            public const string Chinese = "zh-CN";
        }
    }
}
