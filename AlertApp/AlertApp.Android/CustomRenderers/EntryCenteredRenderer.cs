using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Droid.CustomRenderers;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryCentered), typeof(EntryCenteredRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class EntryCenteredRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Gravity = GravityFlags.CenterHorizontal;
            }

        }
    }
}