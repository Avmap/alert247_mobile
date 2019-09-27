using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Contacts = AlertApp.Model.Api.Contacts;
using AndroidNet = Android.Net;
using Android.Database;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.ContactsService))]
namespace AlertApp.Droid.DependencyService
{
    public class ContactsService : IContacts
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;

        public Contact GetContactDetails(string cellphone)
        {
            AndroidNet.Uri uri = AndroidNet.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri, AndroidNet.Uri.Encode(cellphone));

            string[] projection = {
                BaseColumns.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName,
                  ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri,
                  ContactsContract.Contacts.InterfaceConsts.PhotoUri
            };
            ContentResolver contentResolver = context.ContentResolver;
            ICursor contactCursor = contentResolver.Query(uri, null, null, null, null);
            Contact contact = null;
            try
            {
                if (contactCursor != null && contactCursor.Count > 0)
                {
                    contactCursor.MoveToNext();

                    string name = contactCursor.GetString(contactCursor.GetColumnIndex(projection[1]));
                    string photo = contactCursor.GetString(contactCursor.GetColumnIndex(projection[2]));
                    contact = new Contact { Cellphone = cellphone, FirstName = name, ProfileImageUri = photo };
                }
            }
            finally
            {
                if (contactCursor != null)
                {
                    contactCursor.Close();
                }
            }

            return contact;
        }
    }
}