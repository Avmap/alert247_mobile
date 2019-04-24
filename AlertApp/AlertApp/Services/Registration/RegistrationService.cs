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
            var json = JsonConvert.SerializeObject(new RegisterBody { Cellphone = cellphone, Language = language, Hash = "FoH283gIlH0" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/register", content);
            if (response.Content != null)
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

        public async Task<Response<ConfirmRegistrationResponse>> ConfirmRegistration(string cellphone, string otpVerifcationCode)
        {
            var res = new Response<ConfirmRegistrationResponse>();
            try
            {                
                var json = JsonConvert.SerializeObject(new ConfirmRegistrationBody { Cellphone = cellphone, Otp = otpVerifcationCode });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/confirmRegistration", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<ConfirmRegistrationResponse>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorCode = ex.Message;
                res.Status = "error";
            }
            return res;
        }
    }
}
