using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Alert
{
    public interface IAlertService
    {
        Task<bool> SendAlert(string userID);

        Task<bool> SendPing(string userID);
    }
}
