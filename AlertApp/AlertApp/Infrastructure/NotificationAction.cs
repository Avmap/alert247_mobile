using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public class NotificationAction
    {
        public int Type { get; set; }

        public object Data { get; set; }

        public int NotificationId { get; set; }
        
    }



}
