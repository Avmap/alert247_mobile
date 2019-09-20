using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class AlertRecipient
    {
        [DataMember(Name = "cellphone")]
        public string Cellphone { get; set; }
        [DataMember(Name = "profiledata")]
        public string Profiledata { get; set; }
        [DataMember(Name = "filekey")]
        public string Filekey { get; set; }
    }
}
