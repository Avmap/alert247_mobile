using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Essentials;

namespace AlertApp.Droid
{
    [Service(Label = "GuardianService", Enabled = true, Exported = true)]
    public class Guardian : Service
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private LocationRequest locationRequest;
        private FusedLocationProviderCallback locationCallback;
        NotificationCompat.Builder builder;
        string ChannelId = "alert_247_channel";
        Thread t;
        public override void OnCreate()
        {
            base.OnCreate();
            builder = new NotificationCompat.Builder(this);
            ConfigLocationUpdates();
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            this.StartForeground(67236723, GetNotification());
            new AccelerometerTest().ToggleAccelerometer();
            StartLocationUpdates();
            //t = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        bool isLocationEnabled = Utils.isLocationEnabled(this);
            //        Thread.Sleep(1000);
            //    }
            //});
            //t.Start();

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopLocationUpdates();
            //t.Stop();
        }

        Notification GetNotification()
        {
            Intent intent = new Intent();

            //var text = Utils.GetLocationText(Location);

            //// Extra to help us figure out if we arrived in onStartCommand via the notification or not.
            //intent.PutExtra(ExtraStartedFromNotification, true);

            // The PendingIntent that leads to a call to onStartCommand() in this service.
            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            mainActivityIntent.SetAction("stop.service");
            mainActivityIntent.PutExtra("test", true);
            // The PendingIntent to launch activity.
            var activityPendingIntent = PendingIntent.GetActivity(this, 0, mainActivityIntent, PendingIntentFlags.UpdateCurrent);


            builder
                .AddAction(Resource.Mipmap.icon, "Open App",
                    activityPendingIntent)
                //.AddAction(Resource.Mipmap.icon, "stop updates",
                //    servicePendingIntent)
                .SetContentText("Alert247")
                .SetContentTitle("You are protected")
                .SetOngoing(true)
                .SetPriority((int)NotificationPriority.High)
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetTicker("ticker")
                .SetWhen(JavaSystem.CurrentTimeMillis());

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                builder.SetChannelId(ChannelId);
                NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
                NotificationChannel mChannel = new NotificationChannel(ChannelId, "AlertGuardian", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
            }

            return builder.Build();
        }

        private void ConfigLocationUpdates()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetInterval(5000);
            locationRequest.SetFastestInterval(500);
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetSmallestDisplacement(5);
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            locationCallback = new FusedLocationProviderCallback(this);
        }

        private void StartLocationUpdates()
        {
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest, locationCallback, null /* Looper */);
        }

        public void StopLocationUpdates()
        {
            if (fusedLocationProviderClient != null && locationCallback != null)
                fusedLocationProviderClient.RemoveLocationUpdates(locationCallback);
        }

    }


    public class AccelerometerTest
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.UI;

        public AccelerometerTest()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            // Process Acceleration X, Y, and Z
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (System.Exception ex)
            {
                // Other error has occurred.
            }
        }
    }

    public class FusedLocationProviderCallback : LocationCallback
    {
        readonly Context context;

        public FusedLocationProviderCallback(Context context)
        {
            this.context = context;
        }

        public override void OnLocationAvailability(LocationAvailability locationAvailability)
        {
            Log.Debug("FusedLocationProviderSample", "IsLocationAvailable: {0}", locationAvailability.IsLocationAvailable);
        }


        public override void OnLocationResult(LocationResult result)
        {
            if (result.Locations.Any())
            {
                var location = result.LastLocation;
              //  Toast.MakeText(context, "New location", ToastLength.Short).Show();
            }
            else
            {

            }
        }
    }

}
