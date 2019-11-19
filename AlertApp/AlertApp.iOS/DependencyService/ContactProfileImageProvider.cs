using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.DependencyService.ContactProfileImageProvider))]
namespace AlertApp.iOS.DependencyService
{
    public class ContactProfileImageProvider : IContactProfileImageProvider
    {
        public ImageSource GetProfileImage(string uri)
        {
            if (!string.IsNullOrWhiteSpace(uri))
            {
                return ImageSource.FromUri(new Uri(uri));
            }
            return null;
        }
    }
}