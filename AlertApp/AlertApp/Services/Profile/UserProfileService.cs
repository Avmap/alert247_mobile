using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Profile
{
    public class UserProfileService : BaseService, IUserProfileService
    {
        public async Task<Response> Ping(string token, double? lat, double? lng)
        {
            var json = JsonConvert.SerializeObject(new PingUserBody { Token = token, Lat = lat, Lng = lng });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/ping", content);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                if (data != null)
                {
                    var apiResponse = await data.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response>(apiResponse);
                    }
                }
            }

            return Response.FailResponse;
        }

        public async Task<Response> StoreProfile(Dictionary<string, string> registrationValues,string token,string publicKey)
        {
            var json = JsonConvert.SerializeObject(new UserProfileBody { Token  = token,PublicKey = publicKey,ProfileData = "test data"});
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/storeProfile", content);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                if (data != null)
                {
                    var apiResponse = await data.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response>(apiResponse);
                    }
                }
            }
            return Response.FailResponse;
        }
    }
}
