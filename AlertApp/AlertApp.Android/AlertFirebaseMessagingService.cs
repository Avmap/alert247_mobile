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

        /**
         * Called when message is received.
         */

        public override void OnMessageReceived(RemoteMessage message)
        {

            try
            {

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
                        SendAlertNotification(msgT ?? "", msgB ?? "", profiledata ?? "", filekey ?? "", messageType, alertType,position,cellphone);

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void showAlert()
        {


            IWindowManager windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();
            if (windowManager != null)
            {
                ImageView overlayImage = new ImageView(this);
                overlayImage.SetImageResource(Resource.Drawable.alert_fire);

                var param = new WindowManagerLayoutParams(
                                ViewGroup.LayoutParams.MatchParent,
                                ViewGroup.LayoutParams.MatchParent,
                                WindowManagerTypes.SystemOverlay,
                                WindowManagerFlags.Fullscreen | WindowManagerFlags.WatchOutsideTouch | WindowManagerFlags.AllowLockWhileScreenOn | WindowManagerFlags.NotTouchable | WindowManagerFlags.NotFocusable,
                                Format.Translucent);

                param.Gravity = GravityFlags.Top;

                overlayImage.SetScaleType(ImageView.ScaleType.FitXy);

                windowManager.AddView(overlayImage, param);

            }

            //View mView = mInflater.inflate(R.layout.score, null);

            //WindowManager.LayoutParams mLayoutParams = new WindowManager.LayoutParams(
            //ViewGroup.LayoutParams.WRAP_CONTENT,
            //ViewGroup.LayoutParams.WRAP_CONTENT, 0, 0,
            //WindowManager.LayoutParams.TYPE_SYSTEM_OVERLAY,
            //WindowManager.LayoutParams.FLAG_SHOW_WHEN_LOCKED
            //    | WindowManager.LayoutParams.FLAG_DISMISS_KEYGUARD
            //    | WindowManager.LayoutParams.FLAG_TURN_SCREEN_ON
            ///* | WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON */,
            //PixelFormat.RGBA_8888);

            //mWindowManager.addView(mView, mLayoutParams);
        }

        void SendNotification(string title, string messageBody)
        {
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
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

        void SendAlertNotification(string title, string messageBody, string profiledata, string fileKey, string messageType, string alertType,string position,string cellphone)
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
                        "Alert 247",
                        NotificationImportance.Max);
                notificationManager.CreateNotificationChannel(channel);
            }
            else
            {
                notificationBuilder.SetPriority((int)NotificationPriority.Max);
            }

            notificationManager.Notify(notificationID /* ID of notification */, notificationBuilder.Build());
        }

        private async Task<ImportContact> GetContact(string cellPhone)
        {
            var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (contactPermissionStatus == PermissionStatus.Granted)
            {
                var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                if (contacts != null)
                {
                    var result = new List<ImportContact>();
                    var contact = contacts.Where(c => c.Number == cellPhone).FirstOrDefault();
                    if (contact != null)
                    {
                        IContactProfileImageProvider _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
                        return new ImportContact(contact, _contactProfileImageProvider);
                    }
                }
            }

            return null;
        }
    }
}
