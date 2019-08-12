using AlertApp.Infrastructure;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model
{
    public class ImportContact : Contact
    {
        IContactProfileImageProvider _profileImageProvider;

        public ImportContact(Contact contact, IContactProfileImageProvider profileImageProvider)
        {
            _profileImageProvider = DependencyService.Get<IContactProfileImageProvider>(); //DependencyService.Get<IContactProfileImageProvider>();
            this.Name = contact.Name;
            this.Number = contact.Number;
            this.Numbers = contact.Numbers;
            this.PhotoUri = contact.PhotoUri;
            this.PhotoUriThumbnail = contact.PhotoUriThumbnail;
        }
        public ImageSource ProfileImageUri => ImageSource.FromUri(new Uri(PhotoUriThumbnail));

        public ImageSource ProfileImage
        {
            get
            {
                var image = _profileImageProvider.GetProfileImage(PhotoUriThumbnail);              
                if (image != null)
                {
                    return image;
                }

                return ImageSource.FromFile("account_circle.png");
            }
        }

        public bool Selected { get; set; }
    }
}
