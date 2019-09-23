using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class CommunityRequestPageViewModel : BaseViewModel
    {

        #region Commands
        private ICommand _AcceptRequestCommand;
        public ICommand AcceptRequestCommand
        {
            get
            {
                return _AcceptRequestCommand ?? (_AcceptRequestCommand = new Command(AcceptRequest, () =>
                {
                    return !Busy;
                }));
            }
        }
        private ICommand _IgnoreRequestCommand;
        public ICommand IgnoreRequestCommand
        {
            get
            {
                return _IgnoreRequestCommand ?? (_IgnoreRequestCommand = new Command(IgnoreRequest, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _BlockRequestCommand;
        public ICommand BlockRequestCommand
        {
            get
            {
                return _BlockRequestCommand ?? (_BlockRequestCommand = new Command(BlockRequest, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties        
        private Contact _contact;

        public Contact Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        public bool HasChange { get; set; }

        #endregion

        public CommunityRequestPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService, Contact contact)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            Contact = contact;
        }

        private async void AcceptRequest()
        {
            SetBusy(true);
            var result = await _contactsService.AcceptAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }

            await NavigationService.PopModalAsync();

            SetBusy(false);
        }

        private async void BlockRequest()
        {
            SetBusy(true);
            var result = await _contactsService.BlockAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }

            await NavigationService.PopModalAsync();

            SetBusy(false);
        }

        private async void IgnoreRequest()
        {
            SetBusy(true);
            var result = await _contactsService.IgnoreAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }

            await NavigationService.PopModalAsync();

            SetBusy(false);
        }
        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)AcceptRequestCommand).ChangeCanExecute();
                ((Command)IgnoreRequestCommand).ChangeCanExecute();
                ((Command)BlockRequestCommand).ChangeCanExecute();
            });
        }
        #endregion
    }
}
