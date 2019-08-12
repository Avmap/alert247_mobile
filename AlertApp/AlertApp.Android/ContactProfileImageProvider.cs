using System;
using System.Collections.Generic;
using System.IO;
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
using Uri = Android.Net.Uri;
using ContentResolver = Android.Content.ContentResolver;

[assembly: Dependency(typeof(AlertApp.Droid.ContactProfileImageProvider))]
namespace AlertApp.Droid
{
    public class ContactProfileImageProvider : IContactProfileImageProvider
    {
        Activity activity => Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
        public ImageSource GetProfileImage(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("content://"))
            {
                var uri = Uri.Parse(path);
                var stream = activity.ContentResolver.OpenInputStream(uri);

                // eventually convert the stream to imagesource for consumption in Xamarin Forms:
                var imagesource = Xamarin.Forms.ImageSource.FromStream(() => stream);
                return imagesource;
            }
            return null;            
        }
    }
}