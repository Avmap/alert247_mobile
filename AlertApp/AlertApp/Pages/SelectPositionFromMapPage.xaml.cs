using AlertApp.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPositionFromMapPage : ContentPage
    {
        public Xamarin.Forms.Maps.Pin _marker { get; set; }

        public SelectPositionFromMapPage()
        {
            InitializeComponent();
            GetCurrentLocation(); 
        }


        private void Map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            if (_marker == null)
            {
                _marker = new Xamarin.Forms.Maps.Pin { Label = "From here", Position = e.Position };
                map.Pins.Add(_marker);
            }
            else
            {
                _marker.Position = e.Position;
            }
                      
        }
        private async Task<LocationResult> GetCurrentLocation()
        {
            try
            {
                var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    locationPermissionStatus = results[Permission.Location];
                }

                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permissions Denied", "Unable to get location.", "OK");                    
                    return new LocationResult { Ok = false, ErroMessage = "Permissions Denied. Unable to get location." };
                }

                //SetBusy(true);

                var location = await Geolocation.GetLastKnownLocationAsync();
              //  SetBusy(false);
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return new LocationResult { Ok = true, Location = location }; ;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            //SetBusy(false);
            return new LocationResult { Ok = false };
        }
    }
}