using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.iOS.CustomRenderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(OtpEntry), typeof(OtpEntryRenderer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class OtpEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextContentType = UITextContentType.OneTimeCode;          
            }
        }
    }
}