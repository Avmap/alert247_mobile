using AlertApp.Infrastructure;
using AlertApp.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryCenterOnly), typeof(EntryCenteredOnlyRenderer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class EntryCenteredOnlyRenderer : EntryRenderer
    {
        //public EntryCenteredOnlyRenderer()
        //{
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.HorizontalAlignment = UIKit.UIControlContentHorizontalAlignment.Center;
            }
        }
    }
}
