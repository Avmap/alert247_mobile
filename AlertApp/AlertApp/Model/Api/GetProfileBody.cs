using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class GetProfileBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
        [DataMember(Name = "userID")]
        public string UserId { get; set; }
    }
}
