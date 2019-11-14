using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Contacts
{
    public class ContactsService : BaseService, IContactsService
    {
        public async Task<Response<bool>> AcceptAdd(string token, string mobilephone)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AcceptAddBody { Token = token, MobileNumber = mobilephone });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/acceptAdd", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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

        public async Task<Response<bool>> BlockAdd(string token, string mobilephone)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AcceptAddBody { Token = token, MobileNumber = mobilephone });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/blockAdd", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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

        public async Task<Response<CheckContactsResponse>> CheckContacts(string token, string[] mobilephones)
        {
            var res = new Response<CheckContactsResponse>();
            try
            {
                var json = JsonConvert.SerializeObject(new CheckContactsPostBody { Token = token, Contacts = mobilephones });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/checkContacts", content);

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    //generic error handling here
                    res.ErrorCode = "Internal server error";
                    res.Status = "error";
                    return res;
                }
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<CheckContactsResponse>>(apiResponse);
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
                    res.ErrorCode = "Internal server error";
                    res.Status = "error";
                    return res;
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

        public async Task<Response<bool>> IgnoreAdd(string token, string mobilephone)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AcceptAddBody { Token = token, MobileNumber = mobilephone });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/ignoreAdd", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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

        public async Task<Response<bool>> RemoveContacts(string token, List<string> mobilephones)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AddContactBody { Token = token, MobileNumbers = mobilephones.ToArray() });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/removeContacts", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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

        public async Task<Response<bool>> BlockNotifier(string token, string mobilephone)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AcceptAddBody { Token = token, MobileNumber = mobilephone });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/blockNotifier", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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

        public async Task<Response<bool>> RemoveNotifiers(string token, List<string> mobilephones)
        {
            var res = new Response<bool>();
            try
            {
                var json = JsonConvert.SerializeObject(new AddContactBody { Token = token, MobileNumbers = mobilephones.ToArray() });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/removeNotifiers", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<bool>>(apiResponse);
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
