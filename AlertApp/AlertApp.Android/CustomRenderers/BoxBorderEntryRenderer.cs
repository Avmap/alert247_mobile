using System.ComponentModel;
using AlertApp.Droid.CustomRenderers;
using AlertApp.Infrastructure;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BoxBorderEntry), typeof(BoxBorderEntryRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class BoxBorderEntryRenderer : EntryRenderer
    {
        public BoxBorderEntryRenderer(Context context) : base(context)
        {

        }

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid", typeof(bool), typeof(BoxBorderEntry), true);


        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            Control?.SetBackgroundResource(Resource.Drawable.BoxBorder);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "IsValid")
            {
                Control?.SetBackgroundResource(((BoxBorderEntry)sender).IsValid ? Resource.Drawable.BoxBorder : Resource.Drawable.BoxBorderError);
            }
        }
    }
}
