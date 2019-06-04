using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class OtpRequestBody : BaseBody
    {
        [DataMember(Name = "cellphone")]
        public string Cellphone { get; set; }
    }
}
