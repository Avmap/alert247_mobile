using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AlertApp.Droid;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using Xamarin.Forms;

namespace AlertApp.Android
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class AlertFirebaseMessagingService : FirebaseMessagingService, ICrossFirebase
    {
        const string TAG = "AlertFirebaseMsgService";

        /**
         * Called when message is received.
         */

        public override void OnMessageReceived(RemoteMessage message)
        {
            // TODO(developer): Handle FCM messages here.
            // If the application is in the foreground handle both data and notification messages here.
            // Also if you intend on generating your own notifications as a result of a received FCM
            // message, here is where that should be initiated. See sendNotification method below.
            //handle messages when foreground and send from firebase console
            try
            {
                if (message.GetNotification() != null)
                {
                    Log.Debug(TAG, "From: " + message.From);
                    Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                    SendNotification(message.GetNotification().Title ?? "FCM message", message.GetNotification().Body ?? "");
                }
                else
                {
                    string msgT = "";
                    message.Data.TryGetValue("MsgT", out msgT);
                    string msgB = "";
                    message.Data.TryGetValue("MsgB", out msgB);

                    msgT = "Notification From Alert247";
                   //  msgB = message.Data.ToString();

                    if (!string.IsNullOrWhiteSpace(msgT) || !string.IsNullOrWhiteSpace(msgB))
                        SendNotification(msgT ?? "", msgB ?? "");

                    MessagingCenter.Send<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), message);
                    SaveNotificationToDatabase(msgT, msgB);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void SaveNotificationToDatabase(string title, string message)
        {
           
        }

        /**
         * Create and show a simple notification containing the received FCM message.
         */
        void SendNotification(string title, string messageBody)
        {
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, "gr.avmap.alert247.WakeLock");
            wl.SetReferenceCounted(false);
            wl.Acquire(8000);
            var channelid = "gr.avmap.alert247";
            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);
            var intent = new Intent(this, typeof(MainActivity));
            intent.SetAction("mycustom.action.test");//here we can send custom actions depends on notification content and type.
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, channelid)
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetContentTitle(title)
                .SetContentText(messageBody)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);
            if (bm != null)
            {
                notificationBuilder.SetLargeIcon(bm);
            }
            var notificationManager = NotificationManager.FromContext(this);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel(channelid,
                        "Alert 247",
                        NotificationImportance.Max);
                notificationManager.CreateNotificationChannel(channel);
            }
            else
            {
                notificationBuilder.SetPriority((int)NotificationPriority.Max);
            }
            int notificationID = (int)(Java.Lang.JavaSystem.CurrentTimeMillis() / 1000L);
            notificationManager.Notify(notificationID /* ID of notification */, notificationBuilder.Build());
        }
    }
}
