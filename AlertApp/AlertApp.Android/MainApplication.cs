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
using Plugin.FirebasePushNotification;

namespace AlertApp.Droid
{
    [Application]
#if DEBUG
    [MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyD0tos4Y8_QAWxkEnYRuKT7ftEfMT_MZpU")]
#else
       [MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyD-QGABb7MdzQ_46ZGWgu5yCFwYHbypx0o")]
#endif
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
                FirebasePushNotificationManager.DefaultNotificationChannelId = "Alert247";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
              FirebasePushNotificationManager.Initialize(this,false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };


        }
    }
}