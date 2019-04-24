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
        public string api_key { get; set; } = "NDTSAM2DAWCYS5MPPNWQ";
    }
}
