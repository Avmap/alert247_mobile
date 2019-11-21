using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.iOS.CustomRenderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(StatusBarRendrerer))]
namespace AlertApp.iOS.CustomRenderers
{
    public class StatusBarRendrerer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            SetStatusBarStyle();
        }

        private void SetStatusBarStyle()
        {
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, true);
            UIApplication.SharedApplication.SetStatusBarHidden(false, true);
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            SetNeedsStatusBarAppearanceUpdate();
        }
    }
}