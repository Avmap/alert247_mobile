using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class ConfirmRegistrationBody : BaseBody
    {
        [DataMember(Name = "cellphone")]
        public string Cellphone { get; set; }

        [DataMember(Name = "otp")]
        public string Otp { get; set; }
    }
}
