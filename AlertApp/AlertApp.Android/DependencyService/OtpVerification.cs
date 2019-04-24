using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Gms.Auth.Api.Phone;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.OtpVerification))]
namespace AlertApp.Droid.DependencyService
{
    public class OtpVerification : IOtpVerification
    {
        Context context => Plugin.CurrentActivity.CrossCurrentActivity.Current.AppContext;
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
            
            //// Listen for success/failure of the start Task. If in a background thread, this
            //// can be made blocking using Tasks.await(task, [timeout]);            
        }
        internal class SuccessListener : Java.Lang.Object, IOnSuccessListener
        {

            public void OnSuccess(Java.Lang.Object result)
            {
               
            }
        }

        internal class FailureListener : Java.Lang.Throwable, IOnFailureListener
        {

            public void OnFailure(Java.Lang.Exception e)
            {
                
            }
        }
    }
}