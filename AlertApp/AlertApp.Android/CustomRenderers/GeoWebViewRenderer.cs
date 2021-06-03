using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using AlertApp.Views;
using AlertApp.Droid.CustomRenderers;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Platform.Android;
using Android.Webkit;

[assembly: ExportRenderer(typeof(GeoWebView), typeof(GeoWebViewRenderer))]
namespace AlertApp.Droid.CustomRenderers    //todo: what about ios webview renderer?
{
    [Obsolete]
    public class GeoWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            Control.Settings.JavaScriptEnabled = true;
            Control.SetWebChromeClient(new MyWebClient());
        }
    }

    public class MyWebClient : WebChromeClient
    {
        public override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
        {
            callback.Invoke(origin, true, false);
        }
    }
}