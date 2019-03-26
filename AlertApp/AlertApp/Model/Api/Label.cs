using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class Label
    {
        [DataMember(Name = "el-GR")]
        public string el_GR { get; set; }

        [DataMember(Name = "en-US")]
        public string en_US { get; set; }
    }

}
