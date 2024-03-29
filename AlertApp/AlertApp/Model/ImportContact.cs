﻿using AlertApp.Infrastructure;
using Plugin.ContactService.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model
{
    public class ImportContact : Contact, INotifyPropertyChanged
    {
        IContactProfileImageProvider _profileImageProvider;
        public ImportContact(Xamarin.Essentials.Contact contact, String number, IContactProfileImageProvider profileImageProvider)
        {
            _profileImageProvider = profileImageProvider;
            this.Name = contact.DisplayName;
            this.Number = number;
            this.PhotoUri = ""; // contact.ProfileImageUri;
            this.PhotoUriThumbnail = ""; //contact.ProfileImageUri;
        }

        public ImportContact(AlertApp.Model.Api.Contact contact, IContactProfileImageProvider profileImageProvider)
        {
            _profileImageProvider = profileImageProvider;
            this.Name = contact.FullName;
            this.Number = contact.Cellphone;
            this.PhotoUri = contact.ProfileImageUri;
            this.PhotoUriThumbnail = contact.ProfileImageUri;
        }

        public ImportContact(Contact contact, IContactProfileImageProvider profileImageProvider)
        {
            _profileImageProvider = profileImageProvider; //DependencyService.Get<IContactProfileImageProvider>();
            this.Name = contact.Name;
            this.Number = contact.Number;
            this.Numbers = contact.Numbers;
            this.PhotoUri = contact.PhotoUri;
            this.PhotoUriThumbnail = contact.PhotoUriThumbnail;
        }

        public bool HasProfileImage => !string.IsNullOrWhiteSpace(PhotoUriThumbnail);
        public bool NoProfileImage => string.IsNullOrWhiteSpace(PhotoUriThumbnail);

        public ImageSource ProfileImageDefault => ImageSource.FromFile("account_circle.png");
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
        
        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                OnPropertyChanged("Selected");
            }
        }

        private bool _NeedsInvitation;
        public bool NeedsInvitation
        {
            get { return _NeedsInvitation; }
            set
            {
                _NeedsInvitation = value;
                OnPropertyChanged("NeedsInvitation");
            }
        }

        public string FormattedNumber
        {
            get
            {
                return GetFormattedNumber(Number);
            }
        }

        public static string GetFormattedNumber(string number)
        {
            string clearedNumber = number.Trim().Replace("-", "").Replace(" ", "").Replace("(","").Replace(")", "");

            if (clearedNumber.StartsWith("+"))
            {
                return clearedNumber;
            }

            if (clearedNumber.StartsWith("00"))
            {
                return clearedNumber.Replace("00", "+");
            }

            if (clearedNumber.Length == 10)
            {
                return "+30" + clearedNumber;
            }

            return clearedNumber;
        }

        public string NormalizedName => !string.IsNullOrWhiteSpace(Name) ? Language.RemoveDiacritics(Name) : "";

        #region INotifyPropertyChanged
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
