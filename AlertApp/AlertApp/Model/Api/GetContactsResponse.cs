using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{   
    [DataContract]
    public class GetContactsResponse
    {   
        [DataMember(Name = "community")]
        public Dictionary<string, string> Community { get; set; }
        [DataMember(Name = "dependants")]
        public Dictionary<string, string> Dependants { get; set; }
        [DataMember(Name = "alertMe")]
        public Dictionary<string, string> AlertMe { get; set; }
        [DataMember(Name = "blocked")]
        public Dictionary<string, string> Blocked { get; set; }
    }
}
