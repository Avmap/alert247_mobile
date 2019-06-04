using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using Newtonsoft.Json;

namespace AlertApp.Services.Profile
{
    public class UserProfileService : BaseService, IUserProfileService
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        readonly ICryptographyService _cryptographyService;
        #endregion

        public UserProfileService(ILocalSettingsService localSettingsService, ICryptographyService cryptographyService)
        {
            _localSettingsService = localSettingsService;
            _cryptographyService = cryptographyService;
        }

        public async Task<Response> Ping(string token, double? lat, double? lng)
        {
            var json = JsonConvert.SerializeObject(new PingUserBody { Token = token, Lat = lat, Lng = lng });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("post/alert/ping", content);
            if (response.Content != null)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                if (apiResponse != null)
                {
                    return JsonConvert.DeserializeObject<Response>(apiResponse);
                }
            }

            return Response.FailResponse;
        }

        public async Task<Response> StoreProfile(Dictionary<string, string> registrationValues, string token, string publicKey)
        {
            var res = new Response();
            try
            {
                var profileDataJson = JsonConvert.SerializeObject(registrationValues);
                var encryptedProfileData = await _cryptographyService.EncryptProfileData(profileDataJson);
                var json = JsonConvert.SerializeObject(new UserProfileBody { Token = token, PublicKey = publicKey, ProfileData = encryptedProfileData });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/storeProfile", content);

                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        res = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (res.IsOk)
                            _localSettingsService.SaveEncryptedProfileData(encryptedProfileData);
                        return res;
                    }
                }

                return Response.FailResponse;
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
