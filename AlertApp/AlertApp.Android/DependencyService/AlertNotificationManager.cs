using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.AlertNotificationManager))]
namespace AlertApp.Droid.DependencyService
{
    public class AlertNotificationManager : INotificationManager
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;
        public void CloseNotification(int id)
        {
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);            
            notificationManager.Cancel(id);
        }

        public void ToastNotification(string text)
        {
            Toast.MakeText(context, text, ToastLength.Long).Show();
        }
    }
}