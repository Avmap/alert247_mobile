using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class AckAlertPostBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
        [DataMember(Name = "lat")]
        public double? Lat { get; set; }
        [DataMember(Name = "lng")]
        public double? Lng { get; set; }

        [DataMember(Name = "type")]
        public int Type { get; set; }

        [DataMember(Name = "displayedTime")]
        public string DisplayedTime { get; set; }

        [DataMember(Name = "alertID")]
        public int AlertId { get; set; }

        [DataMember(Name = "profiledata")]
        public string ProfileData { get; set; }

        [DataMember(Name = "filekey")]
        public string FileKey { get; set; }
    }
}
