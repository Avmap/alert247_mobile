using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class AddContactBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "contacts")]
        public string[] MobileNumbers { get; set; }
    }
}
