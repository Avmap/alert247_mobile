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
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using views = Android.Views;


[assembly: ExportRenderer(typeof(NoUnderlineEditor), typeof(NoUnderlineEditorRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class NoUnderlineEditorRenderer : EditorRenderer
    {
        public NoUnderlineEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {

                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(global::Android.Graphics.Color.Transparent);
                    Control.SetBackgroundDrawable(gd);
                    Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                    //Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.Black));
                }

                var nativeEditText = (global::Android.Widget.EditText)Control;

                //While scrolling inside Editor stop scrolling parent view.
                nativeEditText.OverScrollMode = OverScrollMode.Always;
                nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                nativeEditText.SetOnTouchListener(new DroidTouchListener());

                //For Scrolling in Editor innner area
                Control.VerticalScrollBarEnabled = true;
                Control.MovementMethod = ScrollingMovementMethod.Instance;
                Control.ScrollBarStyle = ScrollbarStyles.InsideInset;
                //Force scrollbars to be displayed
                TypedArray a = Control.Context.Theme.ObtainStyledAttributes(new int[0]);
                InitializeScrollbars(a);
                  a.Recycle();
            }
          
        }


    }
    public class DroidTouchListener : Java.Lang.Object, views.View.IOnTouchListener
    {      
        public bool OnTouch(global::Android.Views.View v, MotionEvent e)
        {
            v.Parent?.RequestDisallowInterceptTouchEvent(true);
            if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(false);
            }
            return false;
        }
    }
}