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

[assembly: ExportRenderer(typeof(EntryCentered), typeof(EntryCenteredRenderer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class EntryCenteredRenderer : EntryRenderer
    {
        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);

        //    if (Control != null)
        //    {
        //        Control.BorderStyle = UITextBorderStyle.None;                
        //        Control.TextAlignment = UITextAlignment.Center;
        //        //Control.TextColor = UIColor.White;
        //    }
        //}
        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
            {
                return;
            }

            if (Control != null)
            {

            }

            var entry = (EntryCentered)Element;
            var textField = new UIBackwardsTextField();
            textField.EditingChanged += OnEditingChanged;
            textField.OnDeleteBackward += (sender, a) =>
            {
                entry.OnDeletePress();
            };
            textField.ShouldChangeCharacters += (sender, range, replacement) =>
             {
                 var newLength = textField.Text.Length + replacement.Length - range.Length;
                 return newLength < 2;
             };
            SetNativeControl(textField);

            textField.EditingDidBegin += TextField_EditingDidBegin;
        }

        private void TextField_EditingDidBegin(object sender, EventArgs e)
        {
            var entry = (EntryCentered)Element;
            if (entry != null)
            {
                entry.OniOsFocused(entry,entry.Id);
            }
        }

        void OnEditingChanged(object sender, EventArgs eventArgs)
        {
            var element = ElementController as EntryCentered;
            //  entry.OnNumberEnter(eventArgs.Event.DisplayLabel.ToString());
            //ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
            element.OnNumberEnter(Control.Text);
        }
    }

    public class UIBackwardsTextField : UITextField
    {
        // A delegate type for hooking up change notifications.
        public delegate void DeleteBackwardEventHandler(object sender, EventArgs e);

        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event DeleteBackwardEventHandler OnDeleteBackward;


        public void OnDeleteBackwardPressed()
        {
            if (OnDeleteBackward != null)
            {
                OnDeleteBackward(null, null);
            }
        }

        public UIBackwardsTextField()
        {
            //BorderStyle = UITextBorderStyle.RoundedRect;
            BorderStyle = UITextBorderStyle.Line;
            TextColor = Color.FromHex("#37474F").ToUIColor();
            TextAlignment = UITextAlignment.Center;
            ClipsToBounds = true;
            KeyboardType = UIKeyboardType.NumberPad;

        }

        public override void DeleteBackward()
        {
            base.DeleteBackward();
            OnDeleteBackwardPressed();
        }
    }

}