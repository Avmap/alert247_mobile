using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Infrastructure;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.CustomRenderers.Dialog))]
namespace AlertApp.iOS.CustomRenderers
{
    public class Dialog : IDialog
    {
        public async Task<object> showInputDialog(string title, string message, AlertApp.Infrastructure.DialogType inputTupe)
        {
            return await showInputDialog(title, message, "", inputTupe);
        }

        public async Task<object> showInputDialog(string title, string message, object text, AlertApp.Infrastructure.DialogType inputTupe)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            UIAlertView alert = new UIAlertView();
            alert.Title = title;
            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.Message = message;

            switch (inputTupe)
            {
                case AlertApp.Infrastructure.DialogType.Text:
                    alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                    break;
                case AlertApp.Infrastructure.DialogType.Password:
                    alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
                    break;
                case AlertApp.Infrastructure.DialogType.Numeric:
                    alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                    alert.GetTextField(0).KeyboardType = UIKeyboardType.DecimalPad;

                    //alert.GetTextField(0).KeyboardType = UIKeyboardType.NumberPad;
                    break;
            }
            if (alert.GetTextField(0) != null && text != null)
            {
                alert.GetTextField(0).Text = text.ToString();
            }
            // alert.GetTextField(0).Text = text.ToString();
            alert.Clicked += (object s, UIButtonEventArgs ev) =>
            {
                if (ev.ButtonIndex == 0)
                {
                    string input = alert.GetTextField(0).Text;
                    tcs.SetResult(input);
                }
            };
            alert.Show();

            return await tcs.Task;
        }

        public Task<object> showInputDialog(string title, string message, string ok, string cancel, DialogType inputTupe)
        {
            throw new NotImplementedException();
        }

        public Task<object> showInputDialog(string title, string message, object text, string ok, string cancel, DialogType inputTupe)
        {
            throw new NotImplementedException();
        }
    }
}