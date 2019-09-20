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
        public int Helped { get; set; }
        [DataMember(Name = "Ignored")]
        public int Ignored { get; set; }
        [DataMember(Name = "Asked")]
        public int Asked { get; set; }
    }
}
