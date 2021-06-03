using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Subscription
{
    class SubscriptionService : BaseService, ISubscriptionService
    {
        public async Task<Response<SubscriptionResponse>> GetSubscriptionInfo(string token)
        {
            var res = new Response<SubscriptionResponse>();
            try
            {
                var json = JsonConvert.SerializeObject(new GetSubscriptionPostBody { Token = token });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/getSubscription", content);
                var apiResponse = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    //generic error handling here
                    res.ErrorCode = "Internal server error";
                    res.Status = "error: "+apiResponse;
                    return res;
                }
                if (response.Content != null)
                {
                    
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<SubscriptionResponse>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorCode = ex.Message;
                res.Status = "error";
                res.IsOnline = false;
            }
            return res;

        }
    }
}
