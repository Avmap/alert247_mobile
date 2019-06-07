using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Contacts
{
    public class ContactsService :  BaseService,IContactsService
    {
        public async Task<Response<AddContactsResponse>> AddContacts(string token, string[] mobilephones)
        {
            var res = new Response<AddContactsResponse>();
            try
            {
                var json = JsonConvert.SerializeObject(new AddContactBody { Token = token, MobileNumbers = mobilephones });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/addContacts", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<AddContactsResponse>>(apiResponse);
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

        public async Task<Response<GetContactsResponse>> GetContacts(string token)
        {
            var res = new Response<GetContactsResponse>();
            try
            { 
                var json = JsonConvert.SerializeObject(new GetContactsPostBody { Token = token });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/getContacts", content);

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    //generic error handling here
                }
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<GetContactsResponse>>(apiResponse);
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
