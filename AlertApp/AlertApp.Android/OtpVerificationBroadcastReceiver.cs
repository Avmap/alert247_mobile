using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Auth.Api.Phone;
using Xamarin.Forms;

namespace AlertApp.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { SmsRetriever.SmsRetrievedAction })]
    public class OtpVerificationBroadcastReceiver : BroadcastReceiver, IOtpMessageNotifier
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (SmsRetriever.SmsRetrievedAction.Equals(intent.Action))
            {
                if (intent.Action != SmsRetriever.SmsRetrievedAction)
                    return;

                var extrasBundleundle = intent.Extras;
                if (extrasBundleundle == null) return;
                var status = (Statuses)extrasBundleundle.Get(SmsRetriever.ExtraStatus);
                switch (status.StatusCode)
                {
                    case CommonStatusCodes.Success:
                        // Get SMS message contents
                        var messageContent = (string)extrasBundleundle.Get(SmsRetriever.ExtraSmsMessage);
                        //send message to application
                        MessagingCenter.Send<IOtpMessageNotifier, OtpMessageReceivedEvent>(this, OtpMessageReceivedEvent.Event, new OtpMessageReceivedEvent { VerificationMessage = messageContent });
                        break;

                    case CommonStatusCodes.Timeout:
                        // Waiting for SMS timed out (5 minutes)
                        //here handle timeout
                        break;
                }
            }
        }
    }
}