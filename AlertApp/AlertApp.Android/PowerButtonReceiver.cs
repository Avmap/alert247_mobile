using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.Droid
{
    public class PowerButtonReceiver : BroadcastReceiver
    {
        public static string WakeLockTag = "gr.avmap.alert247::CpuWakeLock";
        static bool fromWakeLock = false;
        static int Clicked = 0;
        PowerManager.WakeLock cpuWakeLock;
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

            PowerManager pm = (PowerManager)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.GetSystemService(Context.PowerService);
            if (cpuWakeLock == null)
            {
                cpuWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, WakeLockTag);
                cpuWakeLock.SetReferenceCounted(false);
            }

            if (!cpuWakeLock.IsHeld)
                cpuWakeLock.Acquire();

            bool allwaysOn = Preferences.Get(Settings.AlwaysOn, false);
            if (!allwaysOn && cpuWakeLock.IsHeld)
            {
                cpuWakeLock.Release();
            }



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