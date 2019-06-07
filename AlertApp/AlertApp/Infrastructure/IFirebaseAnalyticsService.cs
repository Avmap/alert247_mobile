using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IFirebaseAnalyticsService
    {        
        void LogEvent(string eventName, Dictionary<string, object> paams);

        void LogOpenAppEvent(string param = null);
        void LogOpenSleepEvent(string param = null);
    }
}
