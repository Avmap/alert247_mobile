﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;
using AppCompatAlertDialog = Android.Support.V7.App.AlertDialog;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.CustomRenderers.Dialog))]
namespace AlertApp.Droid.CustomRenderers
{
    public class Dialog : IDialog
    {        
        Activity activity => Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
        public async Task<object> showInputDialog(string title, string message, AlertApp.Infrastructure.DialogType inputTupe)
        {
            return await showInputDialog(title, message, "", inputTupe);
        }

        public async Task<object> showInputDialog(string title, string message, object text, AlertApp.Infrastructure.DialogType inputTupe)
        {
            EditText et = new EditText(activity);
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            var builder = new AppCompatAlertDialog.Builder(activity);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetCancelable(false);
            switch (inputTupe)
            {
                case DialogType.Text:
                    et.InputType = InputTypes.TextFlagNoSuggestions;
                    break;
                case DialogType.Password:
                    et.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                    break;
                case DialogType.Numeric:
                    et.InputType = InputTypes.ClassNumber;
                    break;
                case DialogType.Phone:
                    et.InputType = InputTypes.ClassPhone;
                    break;
            }
            builder.SetView(et);
            builder.SetNegativeButton("Cancel", (sender, e) =>
            {
                ((AppCompatAlertDialog)sender).Dismiss();
            });
            builder.SetPositiveButton("OK", (sender, e) =>
            {
                string input = et.Text;
                tcs.SetResult(input);
            });


            builder.Create().Show();
            return await tcs.Task;
        }


    }
}