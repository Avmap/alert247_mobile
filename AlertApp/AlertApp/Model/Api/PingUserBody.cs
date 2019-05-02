using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{   
    [DataContract]
    public class PingUserBody : BaseBody
    {   
        [DataMember(Name ="token")]
        public string Token { get; set; }
        [DataMember(Name = "lat")]
        public double? Lat { get; set; }
        [DataMember(Name = "lng")]
        public double? Lng { get; set; }
    }
}
