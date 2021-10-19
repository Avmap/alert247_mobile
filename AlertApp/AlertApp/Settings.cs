using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp
{
    public static class CodeSettings
    {
        public const string SubscriptionURL = "https://www.alert247.gr/subscriptions";
        public const string MapURL = "https://alert.mygis.gr/map.html?id=1";

        public const string MapURLEn = "https://alert.mygis.gr/map_en.html?id=2";
        public const string MapURLBg = "https://alert.mygis.gr/map_en.html?id=3";
        public const string MapURLZh = "https://alert.mygis.gr/map_en.html?id=4";
        public const string MapURLFr = "https://alert.mygis.gr/map_en.html?id=5";
        public const string MapURLDe = "https://alert.mygis.gr/map_en.html?id=6";
        public const string MapURLIt = "https://alert.mygis.gr/map_en.html?id=7";
        public const string MapURLRu = "https://alert.mygis.gr/map_en.html?id=8";

        public const string WhatsNewURL = "https://www.alert247.gr/$LANG$/whatsnew";
        public const string StagingAPI = "https://staging.alert247.gr/api/";
        public const string StagingAPIKey = "NDTSAM2DAWCYS5MPPNWQ";
        public const string ProductionAPI = "https://alert247.gr/api/";
        public const string ProductionAPIKey = "N0S16FDLV2LQ6KEYF3E6";
        public const string UserProfilePage = "https://alert247.gr/$LANG$/diaxeirisi-profil?user=sub$MOBILE$@alert247.gr&passw=$PIN$";
#if DEBUG
        public const string AndroidMapsKey = "AIzaSyD0tos4Y8_QAWxkEnYRuKT7ftEfMT_MZpU";
#else
        public const string AndroidMapsKey = "AIzaSyD-QGABb7MdzQ_46ZGWgu5yCFwYHbypx0o";
#endif
    }
}
