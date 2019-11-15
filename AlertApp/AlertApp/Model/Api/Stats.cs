using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class Stats
    {
        [DataMember(Name = "Helped")]
        public string Helped { get; set; }
        [DataMember(Name = "Ignored")]
        public string Ignored { get; set; }
        [DataMember(Name = "Asked")]
        public string Asked { get; set; }
    }
}
