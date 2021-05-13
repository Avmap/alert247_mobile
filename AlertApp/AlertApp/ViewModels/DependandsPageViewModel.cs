using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Community;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class DependandsPageViewModel : BaseViewModel, IHaveContacts
    {
        #region Services
        readonly ICommunityService _communityService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties

        private ObservableCollection<Contact> _Dependands;
        public ObservableCollection<Contact> Dependands
        {
            get
            {
                if (_Dependands == null)
                {
                    _Dependands = new ObservableCollection<Contact>();
                }
                return _Dependands;
            }
            set
            {
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

        public bool NoContacts => Dependands == null || Dependands.Count == 0;
        #endregion

        #region Commands
        private ICommand _GetDependantsCommand;
        public ICommand GetDependantsCommand
        {
            get
            {
                return _GetDependantsCommand ?? (_GetDependantsCommand = new Command(GetDependants, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        public DependandsPageViewModel(ICommunityService communityService, ILocalSettingsService localSettingsService)
        {
            _communityService = communityService;
            _localSettingsService = localSettingsService;
           // SetBusy(true);
            SetRadiusSettingList();
        }

        private void GetDependants()
        {
            SetBusy(true);
            MessagingCenter.Send((BaseViewModel)this, RefreshContactsEvent.Event, new RefreshContactsEvent { });
        }


        private void SetRadiusSettingList()
        {
            RadiusFromSettingsList = new ObservableCollection<RadiusSetting>();
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 1, Name = "Me (dynamic)" });
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 2, Name = "User's current location" });
            RadiusFromSettingsList.Add(new RadiusSetting { Id = 3, Name = "Pick from map" });
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

                if (radiusSetting.Id == 3)
                {
                    await NavigationService.PushAsync(new SelectPositionFromMapPage(), false);
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
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    locationPermissionStatus = results;
                }

                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    showOKMessage("Permissions Denied", "Unable get location.");
                    return new LocationResult { Ok = false, ErroMessage = "Permissions Denied. Unable get location." };
                }

                SetBusy(true);

                var location = await Geolocation.GetLastKnownLocationAsync();
                SetBusy(false);
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
            SetBusy(false);
            return new LocationResult { Ok = false };
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;                
            });
        }

        public void SetContacts(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            if (response != null && response.IsOk)
            {
                var dependants = response.Result.Contacts.Dependants;
                if (dependants != null && dependants.Count > 0)
                {
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        Dependands.Clear();
                        foreach (var item in dependants)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();
                            if (addressBookItem != null)
                            {
                                Dependands.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                Dependands.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }
                        SetBusy(false);
                    }
                    else
                    {
                        var contacts = dependants.Select(c => new Contact { Accepted = c.Accepted, Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        Dependands.Clear();
                        foreach (var item in contacts)
                        {
                            Dependands.Add(item);
                        }
                        SetBusy(false);
                    }
                }
                else
                {
                    Dependands.Clear();
                    SetBusy(false);
                }
            }
            else
            {
                SetBusy(false);
            }
            OnPropertyChanged("NoContacts");
        }
        #endregion
    }
}
