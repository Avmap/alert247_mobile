using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;


namespace AlertApp.Droid
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY", Value = AlertApp.CodeSettings.AndroidMapsKey)]

    class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                Plugin.FirebasePushNotification.FirebasePushNotificationManager.DefaultNotificationChannelId = "Alert247";

                //Change for your default notification channel name here
                Plugin.FirebasePushNotification.FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

              Plugin.FirebasePushNotification.FirebasePushNotificationManager.Initialize(this,false);


            //Handle notification when app is closed here
            Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };


        }
    }
}