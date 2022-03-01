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

        public async Task<Response> DeleteHistory(string token)
        {
            var res = new Response();
            try
            {
                var json = JsonConvert.SerializeObject(new TokenBody { Token = token });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/deleteHistory", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response>(apiResponse);
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

        public async Task<Response<byte[]>> DownloadHistory(string token)
        {
            var res = new Response<byte[]>();
            try
            {
                var json = JsonConvert.SerializeObject(new TokenBody { Token = token });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/getDataHistory", content);

                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    if (apiResponse != null)
                    {
                        byte[] zipFile = StreamToByteArray(apiResponse);
                        res.Result = zipFile;
                        res.Status = "ok";
                        return res;
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

        public async Task<Response<GetProfileResponse>> GetProfile(string token, string userid)
        {
            var res = new Response<GetProfileResponse>();
            try
            {
                var json = JsonConvert.SerializeObject(new GetProfileBody { UserId = userid, Token = token });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/getProfile", content);

                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        res = JsonConvert.DeserializeObject<Response<GetProfileResponse>>(apiResponse);
                        return res;
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

        public async Task<Response> Ping(string token, double? lat, double? lng, string firebaseToken, string deviceToken)
        {
            var res = new Response();
            try
            {
                var sendLocationSetting = _localSettingsService.GetSendLocationSetting();
                if (!sendLocationSetting)
                {
                    lat = null;
                    lng = null;
                }
                var json = JsonConvert.SerializeObject(new PingUserBody { Token = token, Lat = lat, Lng = lng, FirebaseToken = firebaseToken, IOSDeviceToken = deviceToken});
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
            catch (Exception ex)
            {
                res.ErrorCode = ex.Message;
                res.Status = "error";
                res.IsOnline = false;
            }
            return res;
        }

        public async Task<Response> StoreProfile(Dictionary<string, string> registrationValues, string token, string publicKey)
        {
            var res = new Response();
            try
            {
                var name = await _localSettingsService.GetName();
                var surname = await _localSettingsService.GetSurname();

                var profileDataJson = JsonConvert.SerializeObject(registrationValues);
                var encryptedProfileData = await _cryptographyService.EncryptProfileData(profileDataJson);
                var pin = await _localSettingsService.GetApplicationPin();
                var json = JsonConvert.SerializeObject(new UserProfileBody { Token = token, PublicKey = encryptedProfileData.PublicKey, ProfileData = encryptedProfileData.Data, Pin = pin, Name = name, Surname = surname });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/storeProfile", content);

                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)         
                    {
                        res = JsonConvert.DeserializeObject<Response>(apiResponse);
                        if (res.IsOk)
                            _localSettingsService.SaveEncryptedProfileData(encryptedProfileData.Data);
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
