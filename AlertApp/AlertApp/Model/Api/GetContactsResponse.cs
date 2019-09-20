using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class GetContactsResponse
    {
        [DataMember(Name = "contacts")]
        public Contacts Contacts { get; set; }

    }


    public class Contacts
    {

        [DataMember(Name = "community")]
        public List<Contact> Community { get; set; }
        [DataMember(Name = "dependants")]
        public List<Contact> Dependants { get; set; }
        [DataMember(Name = "alertMe")]
        public List<Contact> AlertMe { get; set; }
        [DataMember(Name = "blocked")]
        public List<Contact> Blocked { get; set; }
    }
}
