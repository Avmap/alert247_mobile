using System;
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
using Com.Google.Android.Gms.Auth.Api.Phone;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.OtpVerification))]
namespace AlertApp.Droid.DependencyService
{
    public class OtpVerification : IOtpVerification
    {
        // Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;
        public void RegisterReceiver()
        {
            // Get an instance of SmsRetrieverClient, used to start listening for a matching
            // SMS message.
            //SmsRetrieverClient client = SmsRetriever.GetClient(context /* context */);

            //// Starts SmsRetriever, which waits for ONE matching SMS message until timeout
            //// (5 minutes). The matching SMS message will be sent via a Broadcast Intent with
            //// action SmsRetriever#SMS_RETRIEVED_ACTION.
            //Android.Gms.Tasks.Task task = client.StartSmsRetriever();

            //// Listen for success/failure of the start Task. If in a background thread, this
            //// can be made blocking using Tasks.await(task, [timeout]);
            //task.AddOnSuccessListener(new OnSuccess);
        }
    }
}