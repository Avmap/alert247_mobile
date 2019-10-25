using AlertApp.Services.Contacts;
using AlertApp.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllertApp.Test
{
    public class ContactsServiceTest
    {
        [Test]
        public void AddContact()
        {
            var contactsService = ViewModelLocator.Instance.Resolve<ContactsService>();
            var token = "eyJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1NTk5MDE4NzYsIm5iZiI6MTU1OTkwMTg3NiwiZXhwIjoxNTU5OTA1NDc2LCJpc3MiOiJBbGVydCBzZXJ2ZXIiLCJhdWQiOiJBbGVydCBtb2JpbGUiLCJ1c2VySUQiOiIxNSJ9.xJrFki4UgZGAziCLJU7Cg81fRQ2oHRo0YA0CpLSk2mk";
            var res = contactsService.AddContacts(token, new string[] { "+306983836637" }).Result;
            if (res.IsOk)
            {

            }
        }

        [Test]
        public void GetContacts()
        {
            var contactsService = ViewModelLocator.Instance.Resolve<ContactsService>();
            var token = "eyJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1NjQwNTU0NzUsIm5iZiI6MTU2NDA1NTQ3NSwiZXhwIjoxNTY0MDU5MDc1LCJpc3MiOiJBbGVydCBzZXJ2ZXIiLCJhdWQiOiJBbGVydCBtb2JpbGUiLCJ1c2VySUQiOiIxNSJ9.Z1I7KyPAdBUdPjCBf4nz5ZvzcREKRVzLcs6D025eb9s";
            var res = contactsService.GetContacts(token).Result;
            if (res.IsOk)
            {

            }
        }
        [Test]
        public void testNumberRegex()
        {
            const string phoneRegex = @"^[0-9]*$";
            bool IsValid = (Regex.IsMatch("5645645", phoneRegex));
        }
        [Test]
        public void testCountryCodeRegex()
        {
             string phoneRegex = @"^\+\d+$";
            bool IsValid = (Regex.IsMatch("+564+d", phoneRegex));

        }
    }
}
