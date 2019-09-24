using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class ManageContactsPageViewModel
    {
        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        public ManageContactsPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
        }

        public async Task<Response<GetContactsResponse>> GetContacts()
        {
            var token = await _localSettingsService.GetAuthToken();
            return await _contactsService.GetContacts(token);            
        }
        
    }
}
