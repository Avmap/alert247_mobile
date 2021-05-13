using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using AlertApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace AlertApp.ViewModels
{
    public class SettingsAccountHistoryViewModel : BaseViewModel
    {
        #region Commands

        private ICommand _DownloadCommand;
        public ICommand DownloadCommand
        {
            get
            {
                return _DownloadCommand ?? (_DownloadCommand = new Command(DownLoadData, () =>
                {
                    return !Busy;
                }));
            }
        }


        private ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new Command(DeleteData, () =>
                {
                    return !Busy;
                }));
            }
        }

        #endregion

        #region Services
        readonly IUserProfileService _profileService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        public SettingsAccountHistoryViewModel(IUserProfileService profileService, ILocalSettingsService localSettingsService)
        {
            _profileService = profileService;
            _localSettingsService = localSettingsService;
        }
        private async void DownLoadData()
        {
            var hasPermission = await HasStoragePermission();
            if (!hasPermission)
                return;
            var storage = DependencyService.Get<IStorage>();
            string title = "";
            string message = "";
            SetBusy(true);       
            var response = await _profileService.DownloadHistory(await _localSettingsService.GetAuthToken());
            if (response.IsOk)
            {
                if (storage != null)
                {
                    var filePath = storage.SaveFile("Alert247ProfileData.zip", response.Result);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        showOKMessage(AppResources.Succcess, AppResources.SucccessSaveAccountFile);
                        Device.BeginInvokeOnMainThread(() => NavigationService.PopAsync(false));
                    });

                }
            }
            else if (!response.IsOnline)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    showOKMessage(AppResources.Error, AppResources.NoInternetConnection);
                });
            }

            SetBusy(false);
        }

        private async void DeleteData()
        {
            //var confirm = await showAlertMessage(AppResources.Warning, AppResources.DeleteAccountMessageAlert, AppResources.ContinueDialogButton, AppResources.Cancel);
            //if (!confirm)
            //    return;
            var confirmView = new ConfirmChangeView();
            var page = new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.Confirmation, confirmView);
            page.Disappearing += (sender2, e2) =>
            {
                if (confirmView.Confirmed)
                {
                    Task.Run(async () =>
                    {
                        SetBusy(true);
                        var response = await _profileService.DeleteHistory(await _localSettingsService.GetAuthToken());
                        SetBusy(false);
                        string title = "";
                        string message = "";
                        if (response.IsOk)
                        {
                            title = AppResources.Succcess;
                            message = AppResources.SucccessDeleteHistory;

                        }
                        else if (!response.IsOnline)
                        {
                            title = AppResources.Error;
                            message = AppResources.NoInternetConnection;
                        }
                        else
                        {

                            title = AppResources.Error;
                            message = AppResources.ErrorDeleteHistory;
                        }

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            showOKMessage(title, message);
                            if (response.IsOk)
                            {
                                NavigationService.PopAsync(false);
                            }
                        });


                    });

                }
            };

            await NavigationService.PushModalAsync(page);


        }

        private async Task<bool> HasStoragePermission()
        {

            var storagePermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (storagePermissionStatus != PermissionStatus.Granted)
            {
                var results = await Permissions.RequestAsync<Permissions.StorageWrite>();
                storagePermissionStatus = results;
            }

            if (storagePermissionStatus == PermissionStatus.Granted)
            {
                return true;
            }

            if (storagePermissionStatus != PermissionStatus.Granted)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.PermissionsDenied, AppResources.PermissionsStorageDeniedMessage, "OK");
                return false;
            }

            return false;
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)DownloadCommand).ChangeCanExecute();
                ((Command)DeleteCommand).ChangeCanExecute();
            });
        }
        #endregion
    }
}

