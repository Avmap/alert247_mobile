﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface ILocalize
    {  
        CultureInfo GetCurrentCultureInfo();
        
        void SetLocale(CultureInfo ci);
    }

    /// <summary>
	/// Helper class for splitting locales like
	///   iOS: ms_MY, gsw_CH
	///   Android: in-ID
	/// into parts so we can create a .NET culture (or fallback culture)
	/// </summary>
	public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if (String.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", "platformCultureString"); // in C# 6 use nameof(platformCultureString)

            PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = "";
            }
        }
        public string PlatformString { get; private set; }
        public string LanguageCode { get; private set; }
        public string LocaleCode { get; private set; }
        public override string ToString()
        {
            return PlatformString;
        }
    }
}
