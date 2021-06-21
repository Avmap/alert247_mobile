using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp
{
    public static class CodeSettings
    {
        public const string SubscriptionURL = "https://www.alert247.gr";
        public const string MapURL = "https://alert.mygis.gr/map.html?id=1";
        public const string WhatsNewURL = "https://www.alert247.gr/$LANG$/whatsnew";
        public const string StagingAPI = "https://staging.alert247.gr/api/";
        public const string StagingAPIKey = "NDTSAM2DAWCYS5MPPNWQ";
        public const string ProductionAPI = "https://alert247.gr/api/";
        public const string ProductionAPIKey = "N0S16FDLV2LQ6KEYF3E6";
#if DEBUG
        public const string AndroidMapsKey = "AIzaSyD0tos4Y8_QAWxkEnYRuKT7ftEfMT_MZpU";
#else
        public const string AndroidMapsKey = "AIzaSyD-QGABb7MdzQ_46ZGWgu5yCFwYHbypx0o";
#endif
    }
}
