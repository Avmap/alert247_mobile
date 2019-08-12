using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Infrastructure
{
    public interface IDialog
    {
        Task<object> showInputDialog(string title, string message, AlertApp.Infrastructure.DialogType inputTupe);
        Task<object> showInputDialog(string title, string message, object text, AlertApp.Infrastructure.DialogType inputTupe);
    }
}
