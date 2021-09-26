using AlertApp.Droid.CustomRenderers;
using AlertApp.Infrastructure;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryCenterOnly), typeof(EntryCenteredOnlyRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class EntryCenteredOnlyRenderer : EntryRenderer
    {
        public EntryCenteredOnlyRenderer(Context context) : base(context)
        {
        }

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