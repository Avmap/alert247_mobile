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

    }
    [DataContract]

    public class ErrorDesctiption
    {
        [DataMember(Name = "labels")]
        public Dictionary<string, string> Labels { get; set; }
    }

    [DataContract]
    public class Response
    {
        [DataMember(Name = "error_code")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "error_description")]
        public ErrorDesctiption ErrorDescription { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }

        public bool IsOk => !string.IsNullOrWhiteSpace(Status) && Status.ToLower().Equals("ok");

        public static Response FailResponse => new Response { Status = "error" };

        public bool IsOnline { get; set; }
    }

}
