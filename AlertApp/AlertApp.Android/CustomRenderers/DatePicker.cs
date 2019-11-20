using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using AlertApp.Infrastructure;
using AlertApp.Utils;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(DatePickerLocal), typeof(AlertApp.Droid.CustomRenderers.DatePicker))]
namespace AlertApp.Droid.CustomRenderers
{
    public class DatePicker : DatePickerRenderer
    {
        private Context context;
        string headerDatePatternLocale;
        SimpleDateFormat monthDayFormatLocale;
        bool headerChangeFlag = true;
        public DatePicker(Context context) : base(context)
        {
            this.context = context;
        }
        TextView headerTextView;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            var preferenceLanguage = Xamarin.Essentials.Preferences.Get(Settings.SelectedLanguage, "en");
           

            var  ci = new CultureInfo(preferenceLanguage);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            //var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            //Configuration cfg = new Configuration();
            ////if (!TextUtils.isEmpty(lang))
            //cfg.SetLocale(new Java.Util.Locale(language));
            ////   else
            ////   cfg.locale = Locale.getDefault();
            //Resources res = context.Resources;
            ////context.CreateConfigurationContext(cfg);
            //context.Resources.UpdateConfiguration(cfg, res.DisplayMetrics);
         
            
            //Java.Util.Locale.SetDefault(Java.Util.Locale.Category.Display,cfg.Locale);

            Locale locale = new Locale(ci.TwoLetterISOLanguageName);
            Control.TextLocale = locale;
            Resources.Configuration.SetLocale(locale);
            var config = new Configuration { Locale = locale };
            context.Resources.UpdateConfiguration(config, Forms.Context.Resources.DisplayMetrics);
           // SetHeaderMonthDay(e.NewElement as DatePickerDialog, locale);
        }

        //void SetHeaderMonthDay(DatePickerDialog dialog, Locale locale)
        //{
        //    if (headerTextView == null)
        //    {
        //        // Material Design formatted CalendarView being used, need to do API level check and skip on older APIs
        //        var id = base.Resources.GetIdentifier("date_picker_header_date", "id", "android");
        //        headerTextView = dialog.DatePicker.FindViewById<TextView>(id);
                
        //        monthDayFormatLocale = new SimpleDateFormat("zEMMMd", locale);                
        //        headerTextView.SetTextColor(global::Android.Graphics.Color.Black);
        //        headerTextView.TextChanged += (sender, e) =>
        //        {
        //            headerChangeFlag = !headerChangeFlag;
        //            if (!headerChangeFlag)
        //                return;
        //            SetHeaderMonthDay(dialog, locale);
        //        };
        //    }
        //    var selectedDateLocale = monthDayFormatLocale.Format(new Date((long)dialog.DatePicker.DateTime.ToUniversalTime().Subtract(
        //              new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds));
        //    headerTextView.Text = selectedDateLocale;
        //}
    }
}