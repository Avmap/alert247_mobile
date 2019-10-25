using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            new Language{ Name = "English",NetLanguageName=Codes.English,CountryMobilePrefix = "+44",Flag="en_US.png"},
            new Language{ Name = "Ελληνικά",NetLanguageName=Codes.Greek,CountryMobilePrefix = "+30",Flag="gr_GR.png"},
            new Language{ Name = "Français",NetLanguageName=Codes.French,CountryMobilePrefix = "+33",Flag="fr_FR.png"},
            new Language{ Name = "Deutsch",NetLanguageName=Codes.German,CountryMobilePrefix = "+49",Flag="de_DE.png"},
            new Language{ Name = "Italian",NetLanguageName=Codes.Italian,CountryMobilePrefix = "+39",Flag="it_IT.png"},
            new Language{ Name = "Russian",NetLanguageName=Codes.Russian,CountryMobilePrefix = "+7",Flag="ru_RU.png"},
            new Language{ Name = "Bulgarian",NetLanguageName=Codes.Bulgarian,CountryMobilePrefix = "+359",Flag="bg_BG.png"},
            new Language{ Name = "Chinese",NetLanguageName=Codes.Chinese,CountryMobilePrefix = "+86",Flag="zh_CN.png"}
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

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
