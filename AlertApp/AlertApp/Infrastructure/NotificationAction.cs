using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public class NotificationAction
    {
        public ActionType Type { get; set; }
     
        public object Data { get; set; }


        public enum ActionType { Sos }
    }



}
