using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AlertApp.Droid
{
    public class LocationJobService : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            throw new NotImplementedException();
        }

        public override bool OnStopJob(JobParameters @params)
        {
            //if we return true system will reschedule the job. so we need to check if user has open the location tracking
            return true;
        }
    }
}