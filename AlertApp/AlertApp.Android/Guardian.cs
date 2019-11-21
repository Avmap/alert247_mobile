using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Services.Profile;
using AlertApp.Utils;
using AlertApp.ViewModels;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Graphics;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Text;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;
using Math = System.Math;

namespace AlertApp.Droid
{
    [Service(Label = "GuardianService", Enabled = true, Exported = true)]
    public class Guardian : Service, ISensorEventListener, FallDetector.IFallDetectionListener
    {
        public static int ALARM_CODE = 454541;
        private FusedLocationProviderClient fusedLocationProviderClient;
        private LocationRequest locationRequest;
        private FusedLocationProviderCallback locationCallback;
        private NotificationCompat.Builder builder;
        private ApplicationAccelerometer accelerometer;
        private FallDetector fallDetector;
        string ChannelId = "alert_247_channel";
        private static SoundPool pool = null;
        private static int id = -1;
        private SoundListener _SoundListener;
        private PowerButtonReceiver _PowerButtonReceiver;
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

            fallDetector = new FallDetector(this);
            accelerometer = new ApplicationAccelerometer(this, fallDetector);

            if (Preferences.Get(Settings.FallDetecion, false))
            {
                fallDetector.Initiate();
                accelerometer.ToggleAccelerometer();
            }

            StartLocationUpdates();
            _PowerButtonReceiver = new PowerButtonReceiver();

            //register power button broadcast receiver
            IntentFilter filter = new IntentFilter(Intent.ActionScreenOn);
            filter.AddAction(Intent.ActionScreenOff);
            RegisterReceiver(_PowerButtonReceiver, filter);


            MessagingCenter.Subscribe<BaseViewModel, StartStopFallDetectionEvent>(this, StartStopFallDetectionEvent.Event, (sender, arg) =>
            {
                if (arg.Start)
                {
                    if (Preferences.Get(Settings.FallDetecion, false))
                    {
                        if (fallDetector == null)
                            fallDetector = new FallDetector(this);


                        if (accelerometer == null)
                            accelerometer = new ApplicationAccelerometer(this, fallDetector);


                        fallDetector.Initiate();
                        accelerometer.ToggleAccelerometer();
                    }
                }
                else if (arg.Stop)
                {
                    if (fallDetector != null)
                    {
                        fallDetector = null;
                    }
                    if (accelerometer != null)
                    {
                        accelerometer.Stop();
                        accelerometer = null;
                    }
                }

            });

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopLocationUpdates();
            if (fallDetector != null)
            {
                fallDetector = null;
            }

            if (accelerometer != null)
            {
                accelerometer.Stop();
                accelerometer = null;
            }
            try
            {
                UnregisterReceiver(_PowerButtonReceiver);
                MessagingCenter.Unsubscribe<BaseViewModel, StartStopFallDetectionEvent>(this, StartStopFallDetectionEvent.Event);
            }
            catch (System.Exception)
            {
            }
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
            locationRequest.SetInterval(10 * 60000);
            locationRequest.SetFastestInterval((10 * 60000) / 2);
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

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.Accelerometer)
            {
                if (fallDetector != null)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine($"Start");
                        fallDetector.Protect(e.Timestamp, e.Values[0], e.Values[1], e.Values[2]);
                        System.Diagnostics.Debug.WriteLine($"End");
                    }
                    catch (Java.Lang.Exception ex)
                    {

                    }
                }
            }
        }

        public void OnFallDetected()
        {
            siren(this);
        }
        void siren(Context context)
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ReorderToFront);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(intent);

            if (_SoundListener == null)
            {
                _SoundListener = new SoundListener();
            }
            if (null == pool)
            {
                AudioAttributes aa = new AudioAttributes.Builder().SetLegacyStreamType(Stream.Alarm).Build();
                pool = new SoundPool.Builder().SetMaxStreams(5).SetAudioAttributes(aa).Build();
                pool.SetOnLoadCompleteListener(_SoundListener);
                id = pool.Load(context, Resource.Raw.alarmtest, 1);
            }
            loudest(context);
            pool.Play(id, 1.0f, 1.0f, 1, 3, 1.0f);
        }
        public static void loudest(Context context)
        {
            AudioManager manager = (AudioManager)context.GetSystemService(Context.AudioService);
            int loudest = manager.GetStreamMaxVolume(Stream.Alarm);
            manager.SetStreamVolume(Stream.Alarm, loudest, 0);
        }

    }

    public class SoundListener : Java.Lang.Object, SoundPool.IOnLoadCompleteListener
    {
        public void OnLoadComplete(SoundPool soundPool, int sampleId, int status)
        {
            soundPool.Play(sampleId, 1.0f, 1.0f, 1, 3, 1.0f);
        }
    }


    public class ApplicationAccelerometer
    {
        private HandlerThread _HandlerThread;
        private Handler _Handler;
        private Sensor _Sensor;
        private SensorManager _Sensormanager;
        private FallDetector _FallDetector;
        private Context _Context;
        public ApplicationAccelerometer(Context context, FallDetector fallDetector)
        {
            _Context = context;
            _FallDetector = fallDetector;
            _Sensormanager = (SensorManager)context.GetSystemService(Context.SensorService);
            _Sensor = _Sensormanager.GetDefaultSensor(SensorType.Accelerometer);
            _HandlerThread = new HandlerThread("AccelerometerLogListener");
            _HandlerThread.Start();
            _Handler = new Handler(_HandlerThread.Looper);
        }
        public void Stop()
        {
            Accelerometer.Stop();
            _Sensormanager.Dispose();
            _Sensor.Dispose();
        }
        public void ToggleAccelerometer()
        {
            try
            {
                _Sensormanager.RegisterListener((ISensorEventListener)_Context, _Sensormanager.GetDefaultSensor(SensorType.Accelerometer), SensorDelay.Game, _Handler);

                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                {
                    Accelerometer.Start(SensorSpeed.Game);
                }

            }
            catch (FeatureNotSupportedException)
            {
                // Feature not supported on device
            }
            catch (System.Exception)
            {
                // Other error has occurred.
            }
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
                // Toast.MakeText(context, "New location", ToastLength.Short).Show();
            }
            else
            {

            }
        }
    }

}
