using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Infrastructure;
using Android.Content;
using Android.Gms.Tasks;
using Android.Gms.Auth.Api.Phone;

using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.OtpVerification))]
namespace AlertApp.Droid.DependencyService
{
    public class OtpVerification : IOtpVerification
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;

        public string GetApplicationHash()
        {
            string applicationhash = AppHashKeyHelper.GetAppHashKey(context);
            return !string.IsNullOrWhiteSpace(applicationhash) ? applicationhash : "FoH283gIlH0";
        }

        public void StartSmsRetriever()
        {
            // Get an instance of SmsRetrieverClient, used to start listening for a matching
            // SMS message.
            SmsRetrieverClient client = SmsRetriever.GetClient(context);

            //// Starts SmsRetriever, which waits for ONE matching SMS message until timeout
            //// (5 minutes). The matching SMS message will be sent via a Broadcast Intent with
            //// action SmsRetriever#SMS_RETRIEVED_ACTION.
            var task = client.StartSmsRetriever();

            task.AddOnSuccessListener(new SuccessListener());
            task.AddOnFailureListener(new FailureListener());
        }

        internal class SuccessListener : Java.Lang.Object, IOnSuccessListener
        {
            public void OnSuccess(Java.Lang.Object result)
            {

            }
        }

        internal class FailureListener : Java.Lang.Object, IOnFailureListener
        {
            public void OnFailure(Java.Lang.Exception e)
            {

            }
        }

    }
}