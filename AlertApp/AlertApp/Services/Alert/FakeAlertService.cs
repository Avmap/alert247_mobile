using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Alert
{
    public class FakeAlertService : IAlertService
    {
        public async Task<bool> SendAlert(string userID)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            return true;
        }

        public async Task<bool> SendPing(string userID)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            return true;
        }
    }
}
