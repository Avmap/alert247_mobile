using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class BaseBody
    {
        [DataMember(Name = "api_key")]
        public string api_key { get; set; } = "N0S16FDLV2LQ6KEYF3E6";
       // public string api_key { get; set; } = "NDTSAM2DAWCYS5MPPNWQ";

        
    }
}
