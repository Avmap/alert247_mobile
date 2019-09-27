using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
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
            throw new NotImplementedException();
        }
    }
}