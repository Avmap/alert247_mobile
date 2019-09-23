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

        public string ProfileImageUri { get; set; }

        #region Computed
        public Color BackgroundStatusColor => Accepted ? Color.Green : Color.Orange;
        public string Status => Accepted ? AppResources.Accepted : AppResources.Pending;
        public string FullName => String.Format("{0} {1}", FirstName, LastName);
        public string Title => !string.IsNullOrWhiteSpace(FullName) ? FullName : Cellphone;

        public string NewRequestMessage => String.Format("{0} {1}", Title, AppResources.NewCommunityRequestWantParticipateMessage);

        public string NewRequestMessage2 => String.Format("{0},{1} {2}", AppResources.ByAccepting, Title, AppResources.ByAcceptingWillAbleToHelpYou);
        #endregion

    }
}
