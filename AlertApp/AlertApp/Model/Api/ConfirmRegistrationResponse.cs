using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{   
    [DataContract]
    public class ConfirmRegistrationResponse
    {   
        [DataMember(Name ="token")]
        public string Token { get; set; }

        [DataMember(Name = "userid")]
        public string UserID { get; set; }

        [DataMember(Name = "fields")]
        public RegistrationField[] Fields { get; set; }
        //this is temp.remove when api is ready
        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}
