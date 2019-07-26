using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace AlertApp.Droid
{
    [Service(Label = "GuardianService", Enabled = true, Exported = true)]
    public class Guardian : Service
    {
        string ChannelId = "Guardian_Channel";        
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {            
            this.StartForeground(67236723, GetNotification());
            return StartCommandResult.Sticky;
        }

        Notification GetNotification()
        {
            Intent intent = new Intent();

            //var text = Utils.GetLocationText(Location);

            //// Extra to help us figure out if we arrived in onStartCommand via the notification or not.
            //intent.PutExtra(ExtraStartedFromNotification, true);

            // The PendingIntent that leads to a call to onStartCommand() in this service.
            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            // The PendingIntent to launch activity.
            var activityPendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .AddAction(Resource.Mipmap.icon, "launch activity",
                    activityPendingIntent)
                .AddAction(Resource.Mipmap.icon, "stop updates",
                    servicePendingIntent)
                .SetContentText("running")
                .SetContentTitle("running title")
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetTicker("ticker")
                .SetWhen(JavaSystem.CurrentTimeMillis());

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                builder.SetChannelId(ChannelId);
            }

            return builder.Build();
        }
    }
}