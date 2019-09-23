using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.MessageCenter
{
    public class RefreshContactsEvent
    {
        public static readonly string Event = "RefreshContactsEvent";
        public string VerificationMessage { get; set; }
    }
}
