﻿using System;
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

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.GuardianPlatform))]
namespace AlertApp.Droid.DependencyService
{
    public class GuardianPlatform : IGuardian
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;

        public void StartDetector()
        {
            Detector.initiate(context);
        }

        public void StartGuardianService()
        {
            Intent i = new Intent(context, typeof(Guardian));
            context.StartService(i);
        }

        public void StopGuardianService()
        {
            Intent i = new Intent(context, typeof(Guardian));
            context.StopService(i);
            //Intent intent = new Intent(Activity.this, MyBackgroundService.class);
            //stopService(intent);
            //  Detector.initiate(context);
        }
    }


}