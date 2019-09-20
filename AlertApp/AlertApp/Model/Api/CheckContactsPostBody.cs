using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class CheckContactsPostBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "contacts")]
        public string[] Contacts { get; set; }
    }
}
