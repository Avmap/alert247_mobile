using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Gms.Auth.Api.Phone;


namespace AlertApp.Droid
{
    [BroadcastReceiver(Enabled = true,Exported =true)]
    [IntentFilter(new[] { SmsRetriever.SmsRetrievedAction })]
    public class OtpVerificationBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (SmsRetriever.SmsRetrievedAction.Equals(intent.Action))
            {
                Bundle extras = intent.Extras;
                //Status status = (Status)extras.Get(SmsRetriever.ExtraStatus);

                //switch (status.getStatusCode())
                //{
                //    case CommonStatusCodes.Success:
                //        // Get SMS message contents
                //        String message = (String)extras.Get(SmsRetriever.ExtraSmsMessage);
                //        // Extract one-time code from the message and complete verification
                //        // by sending the code back to your server.
                //        break;
                //    case CommonStatusCodes.Timeout:
                //        // Waiting for SMS timed out (5 minutes)
                //        // Handle the error ...
                //        break;
                //}
            }
        }
    }
}