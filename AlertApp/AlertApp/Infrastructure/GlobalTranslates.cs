using AlertApp.Resx;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public class GlobalTranslates
    {
        public static string AckWillHelp => AppResources.AckWillHelp;
        public static string AckIgnoreHelp => AppResources.AckNotHelp;
        public static string AckSuffix => AppResources.AckSuffix;

        public static string CommunityRequestNotificationTitle => AppResources.CommunityRequestNotificationTitle;
        public static string CommunityRequestNotificationMessage => AppResources.CommunityRequestNotificationMessage;

        public static string StickNotificationGuardianMessage => AppResources.StickNotificationGuardianMessage;
        public static string StickNotificationGuardianButtonOpenApp => AppResources.StickNotificationGuardianButtonOpenApp;
        public static string Cancel => AppResources.Cancel;
        public static string OK => AppResources.OK;


    }
}
