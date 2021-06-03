using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Subscription
{
    public interface ISubscriptionService
    {
        Task<Response<SubscriptionResponse>> GetSubscriptionInfo(string token);
    }
}
