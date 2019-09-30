using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class SendAlertResponse 
    {
        [DataMember(Name = "recipients")]
        public Dictionary<string, string> Recipients { get; set; }
    }
}
