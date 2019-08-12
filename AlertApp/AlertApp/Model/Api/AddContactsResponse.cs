using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{   
    [DataContract]
    public class AddContactsResponse
    {
        [DataMember(Name = "contacts")]
        Dictionary<string, string> Contacts { get; set; }        
    }
}
