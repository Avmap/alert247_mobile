using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Infrastructure
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci = null;
        const string ResourceId = "AlertApp.Resx.AppResources";

        public TranslateExtension()
        {

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var localize = DependencyService.Get<ILocalize>();
                if (localize != null)
                {
                    var preferenceLanguage = Xamarin.Essentials.Preferences.Get(Settings.SelectedLanguage, "en");
                    if (!string.IsNullOrWhiteSpace(preferenceLanguage))
                    {
                        ci = new CultureInfo(preferenceLanguage);
                    }
                    else
                    {
                        ci = localize.GetCurrentCultureInfo();
                    }
                    Resx.AppResources.Culture = ci;
                }
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = temp.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}


