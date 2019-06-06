using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class GetProfileResponse
    {
        [DataMember(Name = "profile")]
        public string Profile;
    }
}
