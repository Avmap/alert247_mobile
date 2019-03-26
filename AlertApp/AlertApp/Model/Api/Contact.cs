using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        public string FullName => String.Format("{0} {1}",FirstName,LastName);
    }
}
