using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Infrastructure
{
    public interface IWizardViewModel
    {
        Task<bool> CanContinue();
        bool CanGoBack();

        object Continue();

        void GoBack();
    }
}
