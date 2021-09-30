using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using Contacts;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.DependencyService.ContactsService))]
namespace AlertApp.iOS.DependencyService
{
    public class ContactsService : IContacts
    {
        public Contact GetContactDetails(string cellphone)
        {
            var contacts = GetContacts();
            return contacts.Where(c => c.FormmattedNumber == cellphone).FirstOrDefault();
        }
    

        public List<Contact> GetContacts()
        {
            var keysTOFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers ,CNContactKey.ThumbnailImageData };
            NSError error;
            var ContainerId = new CNContactStore().DefaultContainerIdentifier;
            using (var predicate = CNContact.GetPredicateForContactsInContainer(ContainerId))
            {
                CNContact[] contactList;
                using (var store = new CNContactStore())
                {
                    contactList = store.GetUnifiedContacts(predicate, keysTOFetch, out error);
                }
                var contacts = new List<Contact>();

                foreach (var item in contactList)
                {
                    var numbers = item.PhoneNumbers;
                    if (numbers != null)
                    {
                        foreach (var item2 in numbers)
                        {
                            contacts.Add(new Contact
                            {
                                FirstName = item.GivenName,
                                LastName = item.FamilyName,
                                Cellphone = item2.Value.StringValue
                                //_id = item2.Value.ValueForKey(new NSString("digits")).ToString()
                            });
                        }
                    }
                }

                return contacts;

            }
        }
    }
}