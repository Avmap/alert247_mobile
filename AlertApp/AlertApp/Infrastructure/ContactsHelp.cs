using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AlertApp.Infrastructure
{
    public class ContactsHelp
    {
        public static async Task<List<ImportContact>> GetAddressbook(IContacts contactsService, IContactProfileImageProvider contactProfileImageProvider)
        {
            var contactPermissionStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                var results = await Permissions.RequestAsync<Permissions.ContactsRead>();
                contactPermissionStatus = results;
            }

            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                return null;
            }
            var contacts = contactsService.GetContacts();
            //var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            if (contacts != null)
            {
                var result = new List<ImportContact>();
                foreach (var item in contacts.Where(c => c.Cellphone != null).OrderBy(c => c.FirstName))
                {
                    result.Add(new ImportContact(item, contactProfileImageProvider));
                }
                return result;

            }

            return null;
        }
    }
}
