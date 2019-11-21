using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.MessageCenter
{
    public class StartStopFallDetectionEvent
    {
        public static readonly string Event = "StartStopFallDetectionEvent";
        public bool Start { get; set; }
        public bool Stop { get; set; }
    }
}
