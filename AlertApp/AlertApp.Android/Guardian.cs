using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Services.Profile;
using AlertApp.ViewModels;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Plugin.FirebasePushNotification;
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
        private ApplicationAccelerometer accelerometer;

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
            accelerometer = new ApplicationAccelerometer(this);
            accelerometer.ToggleAccelerometer();
            StartLocationUpdates();


            return StartCommandResult.Sticky;
        }
        

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopLocationUpdates();
            if (accelerometer != null)
            {
                accelerometer.Stop();
            }
            //t.Stop();
        }

        Notification GetNotification()
        {
            Intent intent = new Intent();

            // The PendingIntent that leads to a call to onStartCommand() in this service.
            var servicePendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.UpdateCurrent);

            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            mainActivityIntent.SetAction("stop.service");
            mainActivityIntent.PutExtra("test", true);
            // The PendingIntent to launch activity.
            var activityPendingIntent = PendingIntent.GetActivity(this, 0, mainActivityIntent, PendingIntentFlags.UpdateCurrent);

            int color = ContextCompat.GetColor(this, Resource.Color.notificationColor);
            builder
                //.AddAction(Resource.Mipmap.icon, "Open App",
                //    activityPendingIntent)
                .AddAction(0, "Open App",
                    activityPendingIntent)

                .SetContentText("Alert 24/7")
                .SetContentTitle("You are protected")
                .SetOngoing(true)
                .SetColor(color)
                .SetPriority((int)NotificationPriority.High)
                          .SetSmallIcon(Resource.Drawable.ic_stat_logo_icon_notification)
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


    public class ApplicationAccelerometer
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.Fastest;
        Context Context;
        public ApplicationAccelerometer(Context context)
        {
            Context = context;
            // Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            // Process Acceleration X, Y, and Z
        }

        public void Stop()
        {
            Accelerometer.Stop();
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                {
                    Accelerometer.Start(speed);
                    Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                }

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

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            //if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            //{
            //    Context.StartActivity(new Intent(Context, typeof(MainActivity)));
            //}
            //else
            //{
            var intent = new Intent(Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ReorderToFront);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(intent);
            // }

        }
    }

    public class FusedLocationProviderCallback : LocationCallback
    {
        readonly Context context;

        private IUserProfileService _userProfileService;
        public IUserProfileService UserProfileService
        {
            get
            {
                if (_userProfileService == null)
                {
                    _userProfileService = ViewModelLocator.Instance.Resolve<IUserProfileService>();
                }
                return _userProfileService;
            }
            set { _userProfileService = value; }
        }
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
                Task.Run(async () =>
                {
                    var location = result.LastLocation;
                    var token = await SecureStorage.GetAsync(AlertApp.Utils.Settings.AuthToken);
                    var firebaseToken = CrossFirebasePushNotification.Current.Token;
                    if (!string.IsNullOrWhiteSpace(firebaseToken))
                    {
                        await UserProfileService.Ping(token, location.Latitude, location.Longitude, firebaseToken);
                    }
                });
                //  Toast.MakeText(context, "New location", ToastLength.Short).Show();
            }
            else
            {

            }
        }
    }

}
