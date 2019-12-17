using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Droid.CustomRenderers;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoUnderlineEntry), typeof(NoUnderlineEntryRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class NoUnderlineEntryRenderer : EntryRenderer
    {
        public NoUnderlineEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);                
                //Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
}