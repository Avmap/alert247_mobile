using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Services.Community;
using AlertApp.Services.Settings;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AlertApp.ViewModels
{
    public class DependandsPageViewModel : BaseViewModel
    {
        #region Services
        readonly ICommunityService _communityService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties
        private Contact[] _Dependands;
        public Contact[] Dependands
        {
            get { return _Dependands; }
            set
            {
                _Dependands = value;
                OnPropertyChanged("Dependands");
            }
        }

        private ObservableCollection<RadiusSetting> _RadiusFromSettingsList;
        public ObservableCollection<RadiusSetting> RadiusFromSettingsList
        {
            get { return _RadiusFromSettingsList; }
            set
            {
                _RadiusFromSettingsList = value;
                OnPropertyChanged("RadiusFromSettingsList");
            }
        }

        #endregion

        public DependandsPageViewModel(ICommunityService communityService, ILocalSettingsService localSettingsService)
        {
            _communityService = communityService;
            _localSettingsService = localSettingsService;
            GetDependands();
            SetRadiusSettingList();
        }

        private void SetRadiusSettingList()
        {
            RadiusFromSettingsList = new ObservableCollection<RadiusSetting>();
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 1, Name = "Me (dynamic)" });
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 2, Name = "User's current location" });
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 3, Name = "Pick from map" });
        }

        private async void GetDependands()
        {
            SetBusy(true);
            var result = await _communityService.GetDependands("123123");
            if (result != null && result.IsOk)
            {
                Dependands = result.Result.Dependants;
            }
            SetBusy(false);
        }

        public async void ToggleSettings(RadiusSetting radiusSetting, Contact contact)
        {
            if (!radiusSetting.Checked)
            {
                if (radiusSetting.Id == 2)
                {
                    var locationResult = await GetCurrentLocation();
                    if (!locationResult.Ok)
                    {
                        return;
                    }
                }

                radiusSetting.Checked = true;
                var other = RadiusFromSettingsList.Where(r => r.Id != radiusSetting.Id).ToList();
                other.ForEach(r => r.Checked = false);
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
                    showOKMessage("Permissions Denied", "Unable get location.");
                    return new LocationResult { Ok = false,ErroMessage = "Permissions Denied. Unable get location." };
                }

                SetBusy(true);

                var location = await Geolocation.GetLastKnownLocationAsync();
                SetBusy(false);
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return new LocationResult { Ok = true, Location = location}; ;
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
            SetBusy(false);
            return new LocationResult { Ok = false};
        }
       
        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
