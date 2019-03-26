using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class Response<T> : Response
    {

        [DataMember(Name = "result")]
        public T Result { get; set; }

        [DataMember(Name = "error_description")]
        public string ErrorDescription { get; set; }
    }

    public class Response
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        public bool IsOk => !string.IsNullOrWhiteSpace(Status) && Status.ToLower().Equals("ok");

        public static Response FailResponse => new Response { Status = "error" };

    }
}
