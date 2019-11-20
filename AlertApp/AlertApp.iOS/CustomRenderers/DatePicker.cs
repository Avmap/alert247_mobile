using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.Utils;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DatePickerLocal), typeof(AlertApp.iOS.CustomRenderers.DatePicker))]
namespace AlertApp.iOS.CustomRenderers
{
    public class DatePicker : DatePickerRenderer
    {
        public DatePicker()
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            var preferenceLanguage = Xamarin.Essentials.Preferences.Get(Settings.SelectedLanguage, "en");
            var date = (UIDatePicker)Control.InputView;
            date.Locale = new Foundation.NSLocale(preferenceLanguage);
        }

    }
}