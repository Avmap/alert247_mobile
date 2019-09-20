using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class CheckContactsResponse
    {
        [DataMember(Name = "contacts")]
        public Dictionary<string, bool> Contacts { get; set; }
    }
}
