using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Model
{
    public class AlertNotificationData
    {
        public string FileKey { get; set; }
        public string ProfileData { get; set; }
        public string Cellphone { get; set; }
        public string Position { get; set; }
        public int AlertType { get; set; }
        public int? AlertId{ get; set; }
        public string AlertTime { get; set; }
        public string PublicKey { get; set; }
    }
}
