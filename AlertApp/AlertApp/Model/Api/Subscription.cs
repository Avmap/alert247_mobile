using AlertApp.Resx;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class SubscriptionResponse
    {
        [DataMember(Name = "subscription")]
        public Subscription Subscription { get; set; }

    }

    [DataContract]
    public class GetSubscriptionPostBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }

    [DataContract]
    public class Subscription
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Start { get; set; }

        [DataMember]
        public string End { get; set; }

        [DataMember]
        public string Package { get; set; }
    }
}
