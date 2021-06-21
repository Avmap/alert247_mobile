using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace AlertApp.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splash_screen);
            TextView version = FindViewById<TextView>(Resource.Id.appversion);
            var appVersion = Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;

            version.Text = "Version " + appVersion;
#if STAGINGAPI
version.Text += " - STAGING";
#else
            version.Text += " - PRODUCTION";
#endif
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            //Settings.AlwaysOn
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var userID = await SecureStorage.GetAsync(Settings.UserId);
            if (!string.IsNullOrWhiteSpace(userID))
            {
                //await Task.Delay(500);
            }
            else
            {
                //await Task.Delay(2000);
            }

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
        }
        public override void OnBackPressed() { }
    }
}