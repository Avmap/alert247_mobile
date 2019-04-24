using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.MessageCenter
{
    public class OtpMessageReceivedEvent
    {
        public static readonly string Event = "OtpMessageReceivedEvent";
        public string VerificationMessage { get; set; }
    }
}
