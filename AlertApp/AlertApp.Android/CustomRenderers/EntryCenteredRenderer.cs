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
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryCentered), typeof(EntryCenteredRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class EntryCenteredRenderer : EntryRenderer
    {
        public EntryCenteredRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Gravity = GravityFlags.CenterHorizontal;
                var entry = (EntryCentered)Element;
                ((EditText)Control).KeyPress += (object sender, KeyEventArgs eventArgs) =>
                {
                    eventArgs.Handled = false;
                    if (eventArgs.Event.Action == KeyEventActions.Down && eventArgs.KeyCode == Keycode.Del)
                    {
                        entry.OnDeletePress();
                        eventArgs.Handled = true;
                    }
                    else if (eventArgs.Event.Action == KeyEventActions.Down && (
                                eventArgs.KeyCode == Keycode.Num0 ||
                                eventArgs.KeyCode == Keycode.Num1 ||
                                eventArgs.KeyCode == Keycode.Num2 ||
                                eventArgs.KeyCode == Keycode.Num3 ||
                                eventArgs.KeyCode == Keycode.Num4 ||
                                eventArgs.KeyCode == Keycode.Num5 ||
                                eventArgs.KeyCode == Keycode.Num6 ||
                                eventArgs.KeyCode == Keycode.Num7 ||
                                eventArgs.KeyCode == Keycode.Num8 ||
                                eventArgs.KeyCode == Keycode.Num9
                                ))
                    {
                        entry.OnNumberEnter(eventArgs.Event.DisplayLabel.ToString());
                        eventArgs.Handled = true;
                    }
                };
                                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(global::Android.Graphics.Color.Gray);
             
                Control.SetCursorVisible(false);
            }

        }
    }
}