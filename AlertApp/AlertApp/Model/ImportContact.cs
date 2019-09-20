using AlertApp.Infrastructure;
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
                string clearedNumber = Number.Trim().Replace("-", "").Replace(" ", "");

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
        }

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
