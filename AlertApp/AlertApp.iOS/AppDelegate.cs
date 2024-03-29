﻿using System;
using System.Collections.Generic;
using System.Linq;
using CarouselView.FormsPlugin.iOS;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();

            CarouselViewRenderer.Init();
            FirebasePushNotificationManager.Initialize(options, true);
            LoadApplication(new App());
            //FirebasePushNotificationManager.Initialize(options);
            ImageCircleRenderer.Init();
            Xamarin.FormsMaps.Init();
            //Firebase.Core.App.Configure();

            // if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            // {
            //     var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
            //         UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
            //     );
            //
            //     app.RegisterUserNotificationSettings(notificationSettings);
            // }

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                var notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge |
                                        UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
            
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var bytes = deviceToken.ToArray<byte>();
            var hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
            var token = string.Join(string.Empty, hexArray);
            
            SecureStorage.SetAsync(Utils.Settings.IOSDeviceToken, token);
            
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }
        
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
