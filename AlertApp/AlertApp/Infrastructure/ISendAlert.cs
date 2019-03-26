using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Infrastructure
{
    public interface ISendAlert
    {
        void SendUserAlert();

        Task<string> GetApplicationPin();
    }
}
