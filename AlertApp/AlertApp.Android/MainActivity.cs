using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Com.Google.Android.Gms.Auth.Api.Phone;
using Java.Security;
using Java.Nio.Charset;
using Java.Util;
using Plugin.FirebasePushNotification;
using Android.Content;
using Plugin.CurrentActivity;
using Firebase.Analytics;

namespace AlertApp.Droid
{
    [Activity(Label = "AlertApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {           
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);



        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            //hanlde action from notification click.
            var action = intent.Action;            
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }             
    }
}