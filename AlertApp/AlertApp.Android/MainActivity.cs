using System;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Com.Google.Android.Gms.Auth.Api.Phone;
using Java.Security;
using Java.Nio.Charset;
using Java.Util;
using Plugin.FirebasePushNotification;
using Android.Content;
using Plugin.CurrentActivity;
using Firebase.Analytics;
using Plugin.Permissions;
using AlertApp.Android;
using Firebase.Messaging;
using Xamarin.Forms;
using AlertApp.Pages;
using AlertApp.Model;

namespace AlertApp.Droid
{
    [Activity(Label = "AlertApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ICrossFirebase
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            
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
            Intent.Flags);

        }

        protected override void OnResume()
        {
            base.OnResume();

        }

        private void handleIntentActions(string action, string fileKey, string profiledata, int notificationId, string position, int alertType,string cellphone, ActivityFlags flags)
        {
            if (action != null && action.Contains(AlertFirebaseMessagingService.ACTION_OPEN_SOS) && !flags.HasFlag(ActivityFlags.LaunchedFromHistory))
            {
                var notificationData = new NotificationAction();
                notificationData.Type = NotificationAction.ActionType.Sos;
                notificationData.NotificationId = notificationId;
                notificationData.Data = new AlertNotificationData { FileKey = fileKey, ProfileData = profiledata, Position = position, AlertType = alertType,Cellphone = cellphone };
                MessagingCenter.Send<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), notificationData);
                Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new AlertRespondPage(notificationData));
                Intent = null;
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            //hanlde action from notification click.
            var action = intent.Action;
            handleIntentActions(
           action,
           Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_FILE_KEY),
           Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_PROFILE_DATA),
           Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_NOTIFICATION_ID, 0),
           Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_POSITION),
           Intent.GetIntExtra(AlertFirebaseMessagingService.EXTRA_ALERT_TYPE, 0),
           Intent.GetStringExtra(AlertFirebaseMessagingService.EXTRA_CELLPHONE),
           Intent.Flags);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}