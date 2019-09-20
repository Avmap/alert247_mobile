using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlertApp.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class Boot : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != null && intent.Action == Intent.ActionBootCompleted)
            {
                try
                {
                    //Settings.AlwaysOn
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);                
                    if (prefs.GetBoolean(Settings.AlwaysOn, false))
                    {
                        Intent i = new Intent(context, typeof(Guardian));
                        if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
                        {
                            context.StartForegroundService(i);
                        }
                        else
                        {
                            context.StartService(i);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}