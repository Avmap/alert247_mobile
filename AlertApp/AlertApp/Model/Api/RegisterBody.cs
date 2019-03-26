using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class RegisterBody
    {
        [DataMember(Name = "cellphone")]
        public string Cellphone { get; set; }
        
        [DataMember(Name = "language")]
        public string Language { get; set; }
    }
}
