using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class GetRegistrationFieldsBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
