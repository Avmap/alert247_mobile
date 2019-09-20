using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    class SendAlertPostBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
        [DataMember(Name = "lat")]
        public double? Lat { get; set; }
        [DataMember(Name = "lng")]
        public double? Lng { get; set; }
        [DataMember(Name = "recipients")]
        public List<AlertRecipient> Recipients { get; set; } = new List<AlertRecipient>();
        [DataMember(Name = "type")]
        public int Type { get; set; }
    }
}