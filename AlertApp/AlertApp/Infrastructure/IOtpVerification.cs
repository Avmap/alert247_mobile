using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IOtpVerification
    {
        void StartSmsRetriever();
        string GetApplicationHash();
    }
}
