using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Analytics;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.FirebaseAnalyticsService))]
namespace AlertApp.Droid.DependencyService
{
    class FirebaseAnalyticsService : IFirebaseAnalyticsService
    {
        FirebaseAnalytics firebaseAnalytics;

        public FirebaseAnalyticsService()
        {
            firebaseAnalytics = FirebaseAnalytics.GetInstance(context);
        }

        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;


        public void LogEvent(string eventName, Dictionary<string, object> paams)
        {
            firebaseAnalytics.LogEvent(eventName, null);
        }

        public void LogOpenAppEvent(string param)
        {
            try
            {
                if (firebaseAnalytics != null)
                {
                    firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.AppOpen, null);
                }
            }
            catch (Exception)
            {

            }

        }

        public void LogOpenSleepEvent(string param = null)
        {
            try
            {
                if (firebaseAnalytics != null)
                {
                    firebaseAnalytics.LogEvent("application_sleep", null);
                }
            }
            catch (Exception)
            {


            }

        }
    }
}