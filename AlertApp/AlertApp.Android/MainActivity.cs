using System;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Java.Security;
using Java.Nio.Charset;
using Java.Util;
using Android.Content;
using Android.Gms.Auth.Api.Phone;
using Plugin.CurrentActivity;
using Firebase.Analytics;
using Plugin.Permissions;
using AlertApp.Android;
using Firebase.Messaging;
using Xamarin.Forms;
using AlertApp.Pages;
using AlertApp.Model;
using ImageCircle.Forms.Plugin.Droid;

namespace AlertApp.Droid
{
    [Activity(Label = "AlertApp", ShowForAllUsers = true, Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ICrossFirebase
    {
        internal static readonly string EXTRA_FALL_DETECTED = "EXTRA_FALL_DETECTED";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App());
            Plugin.FirebasePushNotification.FirebasePushNotificationManager.ProcessIntent(this, Intent);


            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            var action = Intent.Action;
            handleIntentActions(
            action,
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_FILE_KEY),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_PROFILE_DATA),
            Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_NOTIFICATION_ID, 0),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_POSITION),
            Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_ALERT_TYPE, 0),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_CELLPHONE),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_NOTIFICATION_TYPE),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_ALERT_ID),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_ALERT_TIME),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_SENDER_PUBLIC_KEY)
            ,
            Intent.Flags);

        }

        protected override void OnResume()
        {
            base.OnResume();

        }

        public override void OnAttachedToWindow()
        {
            Window.AddFlags(WindowManagerFlags.ShowWhenLocked |
                            WindowManagerFlags.KeepScreenOn |
                            WindowManagerFlags.DismissKeyguard |
                            WindowManagerFlags.TurnScreenOn);
        }

        private void handleIntentActions(string action, string fileKey, string profiledata, int notificationId, string position, int alertType, string cellphone, string notification, string alertId, string alertTime, string senderPublicKey, ActivityFlags flags)
        {
            if (action != null && action.Contains(AlertFirebaseMessagingService.ACTION_OPEN_SOS) && !flags.HasFlag(ActivityFlags.LaunchedFromHistory))
            {
                var notificationData = new NotificationAction();
                notificationData.Type = alertType;
                notificationData.NotificationId = notificationId;
                notificationData.Data = new AlertNotificationData
                {
                    FileKey = fileKey,
                    ProfileData = profiledata,
                    Position = position,
                    AlertType = alertType,
                    Cellphone = cellphone,
                    AlertId = !string.IsNullOrWhiteSpace(alertId) ? Int32.Parse(alertId) : (int?)null,
                    AlertTime = alertTime,
                    PublicKey = senderPublicKey
                };
                // MessagingCenter.Send<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), notificationData);
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new AlertRespondPage(notificationData));
                Intent = null;
            }
            else if (action != null && notification == "contact" && !flags.HasFlag(ActivityFlags.LaunchedFromHistory))
            {
                var contact = new AlertApp.Model.Api.Contact { Cellphone = cellphone, NotificationId = notificationId };
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new CommunityRequestPage(contact));
                Intent = null;
            }
            else if (action != null && action.Contains(EXTRA_FALL_DETECTED) && !flags.HasFlag(ActivityFlags.LaunchedFromHistory))
            {
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new SendingAlertPage(Model.AlertType.Fall), false);
                Intent = null;
            }            
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            //hanlde action from notification click.
            var action = Intent.Action;
            handleIntentActions(
            action,
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_FILE_KEY),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_PROFILE_DATA),
            Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_NOTIFICATION_ID, 0),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_POSITION),
            Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_ALERT_TYPE, 0),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_CELLPHONE),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_NOTIFICATION_TYPE),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_ALERT_ID),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_ALERT_TIME),
            Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_SENDER_PUBLIC_KEY),
            Intent.Flags);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}