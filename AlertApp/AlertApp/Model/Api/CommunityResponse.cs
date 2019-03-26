using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class CommunityResponse
    {
        [DataMember]
        public Contact[] MyCommunity;
    }
}
