using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface INotificationManager
    {
        void CloseNotification(int id);

        void ToastNotification(string text);
    }
}
