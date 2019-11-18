using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Droid;
using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Services.Cryptography;
using AlertApp.Utils;
using AlertApp.ViewModels;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
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
using static AlertApp.Model.Language;
using static Android.App.KeyguardManager;
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
        public const string EXTRA_ALERT_ID = "EXTRA_ALERT_ID";
        public const string EXTRA_ALERT_TIME = "EXTRA_ALERT_TIME";
        public const string EXTRA_SENDER_PUBLIC_KEY = "EXTRA_SENDER_PUBLIC_KEY";


        /**
         * Called when message is received.
        */
        public override void OnMessageReceived(RemoteMessage message)
        {

            try
            {
                //Handler h = new Handler(Looper.MainLooper);
                //h.Post(() => showAlert());

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

                    string alertID = "";
                    message.Data.TryGetValue("alertID", out alertID);

                    string alertTime = "";
                    message.Data.TryGetValue("alertTime", out alertTime);

                    string publicKey = "";
                    message.Data.TryGetValue("pkey", out publicKey);

                    //ack alert response
                    if (messageType.Equals("ack"))
                    {
                        string ackType = "";
                        message.Data.TryGetValue("ackType", out ackType);

                        string title = "";
                        message.Data.TryGetValue("title", out title);

                        string ackTime = "";
                        message.Data.TryGetValue("ackTime", out ackTime);

                        ShowAlertAckNotification(title, profiledata, filekey, ackType, ackTime);
                        return;
                    }

                    //sos alert
                    if (!string.IsNullOrWhiteSpace(messageType) && messageType.Equals("alert") && !string.IsNullOrWhiteSpace(alertType) && alertType == "1")
                        SendAlertNotification(msgT ?? "", msgB ?? "", profiledata ?? "", filekey ?? "", messageType, alertType, position, cellphone, alertID, alertTime, publicKey);

                    //contact request
                    if (!string.IsNullOrWhiteSpace(messageType) && messageType.Equals("contact") && !string.IsNullOrWhiteSpace(cellphone))
                    {
                        string titleNotification = "";
                        string messageNotification = "";
                        var preferenceLanguage = Xamarin.Essentials.Preferences.Get(Settings.SelectedLanguage, "en");
                        switch (preferenceLanguage)
                        {
                            case Codes.Greek:
                                titleNotification = "Νέο αίτημα";
                                messageNotification = "Έχετε αίτημα για κοινότητα";
                                break;
                            case Codes.English:
                                titleNotification = "Community Request";
                                messageNotification = "You have a new community request";
                                break;
                            default:
                                titleNotification = "Community Request";
                                messageNotification = "You have a new community request";
                                break;

                        }
                        SendContactRequestNotification(titleNotification, messageNotification, position, cellphone);
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
            //PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            //  PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
            // wl.SetReferenceCounted(false);
            // wl.Acquire(8000);

            //var builder = new AlertDialog.Builder(this);
            //builder.SetTitle("Alert");
            //builder.SetMessage("Content");
            //builder.SetCancelable(false);
            //builder.SetPositiveButton("Help", (senderAlert, args) => { AlertDialog t = senderAlert as AlertDialog; t.Dismiss(); });

            //AlertDialog alert = builder.Create();
            //if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            //{
            //    alert.Window.SetType(WindowManagerTypes.ApplicationOverlay);
            //}
            //else
            //{
            //    alert.Window.SetType(WindowManagerTypes.Toast);
            //}

            //alert.Show();

            IWindowManager windowManager = GetSystemService(WindowService).JavaCast<IWindowManager>();

            //if (windowManager != null)
            //{
            //    ImageView overlayImage = new ImageView(this);
            //    overlayImage.SetImageResource(Resource.Drawable.alert_fire);

            //    var param = new WindowManagerLayoutParams(
            //                    ViewGroup.LayoutParams.MatchParent,
            //                    ViewGroup.LayoutParams.MatchParent,
            //                    WindowManagerTypes.ApplicationOverlay,
            //                    WindowManagerFlags.ShowWhenLocked
            //                    | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.TurnScreenOn | WindowManagerFlags.DismissKeyguard,
            //                    Format.Rgba8888);

            //    param.Gravity = GravityFlags.Top;

            //    //  overlayImage.SetScaleType(ImageView.ScaleType.FitXy);

            //    overlayImage.Click += (sender, e) =>
            //    {
            //        windowManager.RemoveView(overlayImage);
            //    };

            //    overlayImage.ViewAttachedToWindow += OverlayImage_ViewAttachedToWindow;

            //    windowManager.AddView(overlayImage, param);

            //}

            StartActivity(new Intent(this, typeof(MainActivity)));
        }

        private void OverlayImage_ViewAttachedToWindow(object sender, global::Android.Views.View.ViewAttachedToWindowEventArgs e)
        {
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.Window.AddFlags(WindowManagerFlags.TurnScreenOn
                | WindowManagerFlags.ShowWhenLocked | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.DismissKeyguard);
        }

        void SendAlertNotification(string title, string messageBody, string profiledata, string fileKey, string messageType, string alertType, string position, string cellphone, string alertid, string alertTime, string publicKey)
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
            intent.PutExtra(EXTRA_ALERT_ID, alertid);
            intent.PutExtra(EXTRA_ALERT_TIME, alertTime);
            intent.PutExtra(EXTRA_SENDER_PUBLIC_KEY, publicKey);

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

            StartActivity(intent);
        }

        void SendContactRequestNotification(string title, string messageBody, string position, string cellphone)
        {

            int notificationID = (int)(Java.Lang.JavaSystem.CurrentTimeMillis() / 1000L);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            prefs.Edit().PutInt(cellphone + "_", notificationID).Commit();

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

        async void ShowAlertAckNotification(string title, string profileData, string fileKey, string ackType, string ackTime)
        {
            // "15/11/19 10:16: Ο/Η ΧΧΧ θα βοηθήσει"
            string messageBody = "";
            int notificationID = (int)(Java.Lang.JavaSystem.CurrentTimeMillis() / 1000L);
            if (!string.IsNullOrWhiteSpace(profileData) && !string.IsNullOrWhiteSpace(fileKey))
            {
                var cryptographyService = ViewModelLocator.Instance.Resolve<ICryptographyService>();
                if (cryptographyService != null)
                {
                    var data = await cryptographyService.GetAlertSenderProfileData(profileData, fileKey);

                    if (data != null)
                    {
                        var name = data[RegistrationField.Names.Name];
                        var surname = data[RegistrationField.Names.Surname];
                        DateTime dateTime = DateTime.Parse(ackTime);
                        messageBody = String.Format("{0} O/H {1} θα βοηθήσει", dateTime.ToString("yyyy/MM/dd HH:mm"), surname + " " + name);
                    }
                }

            }


            //create wake lock
            PowerManager pm = (PowerManager)GetSystemService(Context.PowerService);
            PowerManager.WakeLock wl = pm.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, WakeLock);
            wl.SetReferenceCounted(false);
            wl.Acquire(8000);

            Bitmap bm = BitmapFactory.DecodeResource(Resources, Resource.Mipmap.icon);




            //  var pendingIntent = PendingIntent.GetActivity(this, 0 /* Request code */, intent, PendingIntentFlags.UpdateCurrent);

            int color = ContextCompat.GetColor(this, Resource.Color.notificationColor);
            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, Channelid)
                .SetSmallIcon(Resource.Drawable.ic_stat_logo_icon_notification)
                .SetContentTitle(title)
                .SetColor(color)
                .SetContentText(messageBody)
                // .SetStyle(new NotificationCompat.BigTextStyle().BigText(messageBody))
                //.SetOngoing(true)
                .SetSound(defaultSoundUri);

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
