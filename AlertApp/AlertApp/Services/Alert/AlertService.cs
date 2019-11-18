using AlertApp.Model.Api;
using AlertApp.Services.Contacts;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Alert
{
    public class AlertService : BaseService, IAlertService
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        readonly ICryptographyService _cryptographyService;
        readonly IContactsService _contactsService;
        #endregion

        public AlertService(ILocalSettingsService localSettingsService, ICryptographyService cryptographyService, IContactsService contactsService)
        {
            _localSettingsService = localSettingsService;
            _cryptographyService = cryptographyService;
            _contactsService = contactsService;
        }

        public async Task<Response> AckAlert(string token, double? lat, double? lng, int type, int alertId, DateTime displayedTime)
        {
            var res = new Response();
            var body = new AckAlertPostBody
            {
                Token = token,
                Lat = lat,
                Lng = lng,
                Type = type,
                AlertId = alertId,
                DisplayedTime = displayedTime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            try
            {
                var encryptedUserProfileData = await _localSettingsService.GetEncryptedProfileData();
                var decryptedUserProfileData = await _cryptographyService.DecryptProfileData(encryptedUserProfileData);
                //if (decryptedUserProfileData != null)
                //{
                //    var contacts = await _contactsService.GetContacts(token);
                //    if (contacts != null && contacts.Result != null)
                //    {
                //        foreach (var contact in contacts.Result.Contacts.Community)
                //        {
                //            var recipient = await _cryptographyService.GetAlertRecipient(decryptedUserProfileData, contact);
                //            if (recipient != null)
                //            {
                //                body.Recipients.Add(recipient);
                //            }
                //        }
                //    }

                //}

                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/ackAlert", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<SendAlertResponse>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    res.ErrorCode = ex.Message;
                    res.Status = "error";
                    res.IsOnline = false;
                }
                else
                {
                    res.ErrorCode = ex.Message;
                    res.Status = "error";
                }

            }
            return res;
        }

        public async Task<Response<SendAlertResponse>> SendAlert(string token, double? lat, double? lng, int type)
        {
            var res = new Response<SendAlertResponse>();
            var body = new SendAlertPostBody();
            body.Token = token;
            body.Lat = lat;
            body.Lng = lng;
            body.Type = type;
            try
            {
                var encryptedUserProfileData = await _localSettingsService.GetEncryptedProfileData();
                var decryptedUserProfileData = await _cryptographyService.DecryptProfileData(encryptedUserProfileData);
                if (decryptedUserProfileData != null)
                {
                    var contacts = await _contactsService.GetContacts(token);
                    if (contacts != null && contacts.Result != null)
                    {
                        foreach (var contact in contacts.Result.Contacts.Community)
                        {
                            var recipient = await _cryptographyService.GetAlertRecipient(decryptedUserProfileData, contact);
                            if (recipient != null)
                            {
                                body.Recipients.Add(recipient);
                            }
                        }
                    }

                }

                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("post/alert/sendAlert", content);
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<SendAlertResponse>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Net.Http.HttpRequestException)
                {
                    res.ErrorCode = ex.Message;
                    res.Status = "error";
                    res.IsOnline = false;
                }
                else
                {
                    res.ErrorCode = ex.Message;
                    res.Status = "error";
                }

            }
            return res;
        }
    }
}
