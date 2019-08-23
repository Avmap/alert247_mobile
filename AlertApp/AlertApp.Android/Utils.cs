using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;

namespace AlertApp.Droid
{
    public class Utils
    {
        public static bool isLocationEnabled(Context context)
        {
            if ((int)Build.VERSION.SdkInt >= 28)
            {
                // This is new method provided in API 28
                LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);
                return lm.IsProviderEnabled(LocationManager.GpsProvider);
            }
            else
            {
                // This is Deprecated in API 28
                int mode = Settings.Secure.GetInt(context.ContentResolver, Settings.Secure.LocationMode, (int)SecurityLocationMode.Off);
                return (mode != (int)SecurityLocationMode.Off);

            }
        }

        public static bool hasLocationPermission()
        {
            return false;
        }
    }
}