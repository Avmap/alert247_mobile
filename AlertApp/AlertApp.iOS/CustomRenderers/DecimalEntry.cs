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

[assembly: ExportRenderer(typeof(DecimalEntry), typeof(DecimalEntryRenderer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class DecimalEntryRenderer : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                if (((DecimalEntry)Element).SupportNegativeValues)
                {
                    Control.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
                }
                else
                {
                    Control.KeyboardType = UIKeyboardType.DecimalPad;
                }                
            }
        }
    }
}