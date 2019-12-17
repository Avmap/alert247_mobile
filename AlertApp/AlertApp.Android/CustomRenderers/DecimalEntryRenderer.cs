using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(AlertApp.Infrastructure.DecimalEntry), typeof(AlertApp.Droid.CustomRenderers.DecimalEntryRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class DecimalEntryRenderer : NoUnderlineEntryRenderer
    {
        public DecimalEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                //  this.Control.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance(Resources.Configuration.Locale, true, true);
                string manufacturer = Build.Manufacturer;
                this.Control.KeyListener = DigitsKeyListener.GetInstance(true, true);
                //if (!string.IsNullOrWhiteSpace(manufacturer) && manufacturer.ToLower().Contains("samsung"))
                //{
                //    this.Control.InputType = InputTypes.ClassPhone | InputTypes.NumberFlagSigned | InputTypes.NumberFlagDecimal;
                //}
                //else
                //{              
                this.Control.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagSigned | InputTypes.NumberFlagDecimal;
                // }
            }
        }
    }
}