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

namespace AlertApp.Droid
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine($"Alarm Triggered");
            Toast.MakeText(context, "Alarm Triggered", ToastLength.Long).Show();
        }
    }
}