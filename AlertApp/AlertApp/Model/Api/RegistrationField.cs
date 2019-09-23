using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AlertApp.Model.Api
{
    [DataContract]
    public class RegistrationField
    {
        public static class Type
        {
            public const string String = "string";
            public const string Date = "date";
            public const string Integer = "integer";
            public const string Bool = "bool";
            public const string Boolean = "boolean";
        }
        public static class Name
        {
            public const string FullName = "fullname";
            public const string FirstName = "firstname";
            public const string Surname = "surname";
            public const string Area = "area";
            public const string BirthDate = "birthdate";            
        }

        [DataMember(Name = "field")]
        public string FieldName { get; set; }
        [DataMember(Name = "type")]
        public string DataType { get; set; }
        [DataMember(Name = "labels")]
        public Dictionary<string, string> Labels { get; set; }
    }


}
