using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Infrastructure
{
    public interface IDialog
    {
        Task<object> showInputDialog(string title, string message,string ok , string cancel, AlertApp.Infrastructure.DialogType inputTupe);
        Task<object> showInputDialog(string title, string message, object text, string ok, string cancel, AlertApp.Infrastructure.DialogType inputTupe);
    }
}
