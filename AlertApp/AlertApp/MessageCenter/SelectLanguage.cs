using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.MessageCenter
{
    public class SelectLanguage
    {
        public static readonly string Event = "SelectLanguage";

        public Language Language { get; set; }
    }
}
