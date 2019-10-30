using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.DependencyService.AlertNotificationManager))]
namespace AlertApp.iOS.DependencyService
{
    public class AlertNotificationManager : INotificationManager
    {
        
        public void CloseNotification(int id)
        {
          //  NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
           // notificationManager.Cancel(id);
        }

        public void ToastNotification(string text)
        {
            var alertController = UIAlertController.Create("", text, UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (action) => Console.WriteLine("OK Clicked.")));

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);
            // Toast.MakeText(context, text, ToastLength.Long).Show();
        }
    }
}