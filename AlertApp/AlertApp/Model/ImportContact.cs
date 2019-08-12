using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model
{
    public class ImportContact : Contact
    {
        public ImportContact(Contact contact)
        {
            this.Name = contact.Name;
            this.Number = contact.Number;
            this.Numbers = contact.Numbers;
            this.PhotoUri = contact.PhotoUri;
            this.PhotoUriThumbnail = contact.PhotoUriThumbnail;
        }
        public ImageSource ProfileImageUri => ImageSource.FromUri(new Uri(PhotoUriThumbnail));

        public bool Selected { get; set; }
    }
}
