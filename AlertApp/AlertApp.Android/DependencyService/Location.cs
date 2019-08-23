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

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.Location))]
namespace AlertApp.Droid.DependencyService
{
    public class Location : ILocationSettings
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;
        public bool IsLocationEnabled()
        {
            return Utils.isLocationEnabled(context);
        }
    }
}