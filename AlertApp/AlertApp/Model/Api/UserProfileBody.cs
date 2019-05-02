﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class UserProfileBody : BaseBody
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "public_key")]
        public string PublicKey { get; set; }

        [DataMember(Name = "profile")]
        public string ProfileData { get; set; }
    }
}
