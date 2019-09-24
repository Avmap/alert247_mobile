using AlertApp.Model;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Infrastructure
{
    public class ContactsHelp
    {
        public static async Task<List<ImportContact>> GetAddressbook(IContactProfileImageProvider contactProfileImageProvider)
        {
            var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Contacts });
                contactPermissionStatus = results[Permission.Contacts];
            }

            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                return null;
            }

            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            if (contacts != null)
            {
                var result = new List<ImportContact>();
                foreach (var item in contacts.Where(c => c.Number != null).OrderBy(c => c.Name))
                {
                    result.Add(new ImportContact(item, contactProfileImageProvider));
                }
                return result;

            }

            return null;
        }
    }
}
