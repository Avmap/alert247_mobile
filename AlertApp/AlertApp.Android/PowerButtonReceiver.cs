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
using Java.Lang;
using Xamarin.Forms;

namespace AlertApp.Droid
{
    public class PowerButtonReceiver : BroadcastReceiver
    {
        static bool fromWakeLock = false;
        static int Clicked = 0;

        public override void OnReceive(Context context, Intent intent)
        {
            if (fromWakeLock)
            {
                Clicked = 0;
                return;
            }

            Clicked++;

            if (Clicked == 2)
            {
                OpenApp();
                Clicked = 0;
            }            
            new ClickClearTimer(1000, 500).Start();
        }

        private void OpenApp()
        {
            PowerManager pm = (PowerManager)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, "WakeLock");
            wl.SetReferenceCounted(false);
            fromWakeLock = true;
            wl.Acquire(8000);
            var intent = new Intent(Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ReorderToFront);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(intent);
            fromWakeLock = false;
        }

        public class ClickClearTimer : CountDownTimer
        {

            public ClickClearTimer(long millisInFuture, long countDownInterva) : base(millisInFuture, countDownInterva)
            {

            }

            public override void OnFinish()
            {                
                Clicked = 0;
            }

            public override void OnTick(long millisUntilFinished)
            {

            }

        }
    }

}