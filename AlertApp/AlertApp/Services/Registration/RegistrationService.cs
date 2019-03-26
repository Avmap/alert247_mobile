using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Registration
{
    public class RegistrationService : BaseService, IRegistrationService
    {
        public async Task<Response> Register(string cellphone, string language)
        {
            var json = JsonConvert.SerializeObject(new RegisterBody { Cellphone = cellphone, Language = language });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/register", content);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content;
                if (data != null)
                {
                    var apiResponse = await data.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response>(apiResponse) ;
                    }
                }
            }

            return Response.FailResponse;
        }

        public async Task<ConfirmRegistrationResponse> ConfirmRegistration(string cellphone, string otpVerifcationCode)
        {
            var json = JsonConvert.SerializeObject(new ConfirmRegistrationBody { Cellphone = cellphone, Otp = otpVerifcationCode });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/confirmRegistration", content);
            if (response.IsSuccessStatusCode)
            {                
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<ConfirmRegistrationResponse>(apiResponse);
                    }
                }
            }        
            return null;
        }
    }
}
