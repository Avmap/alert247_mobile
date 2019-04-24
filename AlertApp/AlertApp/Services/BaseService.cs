using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AlertApp.Services
{
    public abstract class BaseService
    {
        //protected string Url => "https://private-anon-b9057d6115-alert247.apiary-mock.com/api/";
        protected string Url => "https://staging.alert247.gr/api/ ";
        
        protected readonly HttpClient _httpClient = new HttpClient();
        public BaseService()
        {
            _httpClient.BaseAddress = new Uri(Url);
        }
    }
}
