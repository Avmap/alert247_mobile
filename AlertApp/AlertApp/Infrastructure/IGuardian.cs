using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IGuardian
    {
        void StartGuardianService();
        void StopGuardianService();
    }
}
