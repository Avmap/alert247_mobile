using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class DependantsResponse
    {
        [DataMember]
        public Contact[] Dependants;
    }
}
