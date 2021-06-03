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
#if STAGINGAPI
        public string api_key { get; set; } = AlertApp.CodeSettings.StagingAPIKey; //staging api key
#else
        public string api_key { get; set; } = AlertApp.CodeSettings.ProductionAPIKey; //production api key
#endif


    }
}
