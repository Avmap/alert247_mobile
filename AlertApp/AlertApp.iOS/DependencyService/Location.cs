using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using CoreLocation;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.DependencyService.Location))]
namespace AlertApp.iOS.DependencyService
{
    public class Location : ILocationSettings
    {
        public bool IsLocationEnabled()
        {
            if (CLLocationManager.Status == CLAuthorizationStatus.Denied)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
