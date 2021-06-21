using AlertApp.Resx;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class Contact
    {

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember(Name = "cellphone")]
        public string Cellphone { get; set; }
        [DataMember(Name = "public_key")]
        public string PublicKey { get; set; }

        [DataMember(Name = "accepted")]
        public bool Accepted { get; set; }

        [DataMember(Name = "stats")]
        public Stats Stats { get; set; }

        public ImageSource ProfileImage { get; set; }
        public ImageSource ProfileImageDefault => ImageSource.FromFile("account_circle.png");
        public string ProfileImageUri { get; set; }

        #region Computed
        public bool IsPending => !Accepted;

        public bool HasProfileImage => !string.IsNullOrWhiteSpace(ProfileImageUri);
        public bool NoProfileImage => string.IsNullOrWhiteSpace(ProfileImageUri);
        public int NotificationId { get; set; }
        public string StatusAlertMe => Accepted ? AppResources.AcceptedFromMe : AppResources.Pending;
        public Color BackgroundStatusColor => Accepted ? Color.FromHex("#800000") : Color.FromHex("#DCDDDE");
        public Color ΤextStatusColor => Accepted ? Color.White : Color.FromHex("#800000");
        public string Status => Accepted ? AppResources.Accepted : AppResources.Pending;
        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName))
                {
                    return String.Format("{0} {1}", FirstName, LastName);
                }else if (!string.IsNullOrEmpty(LastName))
                {
                    return LastName;
                }
                else
                {
                    return "";
                }
            }
        }
        
        public string Title => !string.IsNullOrWhiteSpace(FullName) ? FullName : Cellphone;

        public string NewRequestMessage => String.Format("{0} {1}", Title, AppResources.NewCommunityRequestWantParticipateMessage);

        public string NewRequestMessage2 => String.Format("{0},{1} {2}", AppResources.ByAccepting, Title, AppResources.ByAcceptingWillAbleToHelpYou);


        public string FormmattedNumber => ImportContact.GetFormattedNumber(Cellphone);
        #endregion

    }
}
