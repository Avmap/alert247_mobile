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
using Plugin.Permissions;

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
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            var action = Intent.Action;


        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            //hanlde action from notification click.
            var action = intent.Action;            
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}