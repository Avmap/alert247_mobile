using System.ComponentModel;
using AlertApp.Infrastructure;
using AlertApp.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BoxBorderEntry), typeof(BoxBorderEntryRenderer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class BoxBorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;
            Control.Layer.BorderColor = Color.FromHex("#BDBDC3").ToCGColor();
            Control.Layer.BorderWidth = 1;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsValid")
            {
                Control.Layer.BorderColor = ((BoxBorderEntry)sender).IsValid ? ((BoxBorderEntry)sender).NormalBorderColor.ToCGColor() : ((BoxBorderEntry)sender).ErrorBorderColor.ToCGColor();
            }
        }
    }
}
