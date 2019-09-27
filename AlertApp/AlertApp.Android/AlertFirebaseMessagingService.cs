using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Droid;
using AlertApp.Infrastructure;
using AlertApp.Model;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
namespace AlertApp.Android
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class AlertFirebaseMessagingService : FirebaseMessagingService, ICrossFirebase
    {
        const string TAG = "AlertFirebaseMsgService";
        public const string Channelid = "gr.avmap.alert247";
        public const string WakeLock = "gr.avmap.alert247.WakeLock";

        public const string ACTION_OPEN_SOS = "ACTION.OPEN_SOS_ALERT";
        public const string EXTRA_PROFILE_DATA = "EXTRA_PROFILE_DATA";
        public const string EXTRA_FILE_KEY = "EXTRA_FILE_KEY";
        public const string EXTRA_NOTIFICATION_ID = "EXTRA_NOTIFICATION_ID";
        public const string EXTRA_POSITION = "EXTRA_POSITION";
        public const string EXTRA_ALERT_TYPE = "EXTRA_ALERT_TYPE";
        public const string EXTRA_CELLPHONE = "EXTRA_CELLPHONE";
        public const string EXTRA_NOTIFICATION_TYPE = "EXTRA_NOTIFICATION_TYPE";

        /**
         * Called when message is received.
         */

        public override void OnMessageReceived(RemoteMessage message)
        {

            try
            {
                // Handler h = new Handler(Looper.MainLooper);
                // h.Post(() => showAlert());

                if (message.Data != null)
                {

                    string msgT = "";
                    message.Data.TryGetValue("title", out msgT);

                    string msgB = "";
                    message.Data.TryGetValue("body", out msgB);

                    string messageType = "";
                    message.Data.TryGetValue("messageType", out messageType);

                    string profiledata = "";
                    message.Data.TryGetValue("profiledata", out profiledata);

                    string filekey = "";
                    message.Data.TryGetValue("filekey", out filekey);

                    string alertType = "";
                    message.Data.TryGetValue("alertType", out alertType);

                    string position = "";
                    message.Data.TryGetValue("position", out position);

                    string cellphone = "";
                    message.Data.TryGetValue("cellphone", out cellphone);

                    //manual sos alert
                    if (!string.IsNullOrWhiteSpace(messageType) && messageType.Equals("alert") && !string.IsNullOrWhiteSpace(alertType) && alertType == "1")
                        SendAlertNotification(msgT ?? "", msgB ?? "", profiledata ?? "", filekey ?? "", messageType, alertType, position, cellphone);

                    if (!string.IsNullOrWhiteSpace(messageType) && messageType.Equals("contact") && !string.IsNullOrWhiteSpace(cellphone))
                    {
                        SendContactRequestNotification("Community Request", "You have a new community request", position, cellphone);
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void showAlert()
        {

            //create wake lock
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
            wl.SetReferenceCounted(false);
            wl.Acquire(8000);

            var builder = new AppCompatAlertDialog.Builder(this);
            builder.SetTitle("Alert");
            builder.SetMessage("Content");
            builder.SetCancelable(false);
            builder.SetPositiveButton("Help", (senderAlert, args) => { AppCompatAlertDialog t = senderAlert as AppCompatAlertDialog; t.Dismiss(); });

            AppCompatAlertDialog alert = builder.Create();
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                alert.Window.SetType(WindowManagerTypes.ApplicationOverlay);
            }
            else
            {
                alert.Window.SetType(WindowManagerTypes.Toast);
            }

            alert.Show();
            //IWindowManager windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            //if (windowManager != null)
            //{
            //    ImageView overlayImage = new ImageView(this);
            //    overlayImage.SetImageResource(Resource.Drawable.alert_fire);

            //    var param = new WindowManagerLayoutParams(
            //                    ViewGroup.LayoutParams.MatchParent,
            //                    ViewGroup.LayoutParams.MatchParent,
            //                    WindowManagerTypes.SystemAlert,
            //                    WindowManagerFlags.Fullscreen ,
            //                    Format.Translucent);

            //    param.Gravity = GravityFlags.Top;

            //    overlayImage.SetScaleType(ImageView.ScaleType.FitXy);

            //    windowManager.AddView(overlayImage, param);

            //}


        }

      
        void SendAlertNotification(string title, string messageBody, string profiledata, string fileKey, string messageType, string alertType, string position, string cellphone)
        {
            int notificationID = (int)(Java.Lang.JavaSystem.CurrentTimeMillis() / 1000L);
            //create wake lock
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
            wl.SetReferenceCounted(false);
            wl.Acquire(8000);

            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);
            //create pending intent action
            var intent = new Intent(this, typeof(MainActivity));
            //here we can send custom actions depends on notification content and type.
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
            intent.SetAction(ACTION_OPEN_SOS + Java.Lang.JavaSystem.CurrentTimeMillis());
            intent.PutExtra(EXTRA_FILE_KEY, fileKey);
            intent.PutExtra(EXTRA_PROFILE_DATA, profiledata);
            intent.PutExtra(EXTRA_NOTIFICATION_ID, notificationID);
            intent.PutExtra(EXTRA_POSITION, position);
            intent.PutExtra(EXTRA_ALERT_TYPE, Int32.Parse(alertType));
            intent.PutExtra(EXTRA_CELLPHONE, cellphone);
            intent.PutExtra(EXTRA_NOTIFICATION_TYPE, "alert");

            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.UpdateCurrent);
            int color = ContextCompat.GetColor(this, Resource.Color.notificationColor);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, Channelid)
                .SetSmallIcon(Resource.Drawable.ic_stat_logo_icon_notification)
                .SetContentTitle(title)
                .SetColor(color)
                .SetContentText(messageBody)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                .SetOngoing(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            if (bm != null)
            {
                //notificationBuilder.SetLargeIcon(bm);
            }

            var notificationManager = NotificationManager.FromContext(this);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel(Channelid,
                        "Alert 24/7",
                        NotificationImportance.Max);
                notificationManager.CreateNotificationChannel(channel);
            }
            else
            {
                notificationBuilder.SetPriority((int)NotificationPriority.Max);
            }

            notificationManager.Notify(notificationID /* ID of notification */, notificationBuilder.Build());
        }

        void SendContactRequestNotification(string title, string messageBody, string position, string cellphone)
        {
            int notificationID = (int)(Java.Lang.JavaSystem.CurrentTimeMillis() / 1000L);
            //create wake lock
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
            wl.SetReferenceCounted(false);
            wl.Acquire(8000);

            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);
            //create pending intent action
            var intent = new Intent(this, typeof(MainActivity));
            //here we can send custom actions depends on notification content and type.
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop | ActivityFlags.NewTask);
            intent.SetAction(Java.Lang.JavaSystem.CurrentTimeMillis().ToString());            
            intent.PutExtra(EXTRA_NOTIFICATION_ID, notificationID);
            intent.PutExtra(EXTRA_POSITION, position);
            intent.PutExtra(EXTRA_NOTIFICATION_TYPE, "contact");
            intent.PutExtra(EXTRA_CELLPHONE, cellphone);

            var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.UpdateCurrent);
            int color = ContextCompat.GetColor(this, Resource.Color.notificationColor);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, Channelid)
                .SetSmallIcon(Resource.Drawable.ic_stat_logo_icon_notification)
                .SetContentTitle(title)
                .SetColor(color)
                .SetContentText(messageBody)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                .SetOngoing(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent);

            if (bm != null)
            {
                //notificationBuilder.SetLargeIcon(bm);
            }

            var notificationManager = NotificationManager.FromContext(this);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel channel = new NotificationChannel(Channelid,
                        "Alert 24/7",
                        NotificationImportance.Max);
                notificationManager.CreateNotificationChannel(channel);
            }
            else
            {
                notificationBuilder.SetPriority((int)NotificationPriority.Max);
            }

            notificationManager.Notify(notificationID, notificationBuilder.Build());
        }
    }
}
