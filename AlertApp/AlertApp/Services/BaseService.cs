using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace AlertApp.Services
{
    public abstract class BaseService
    {
#if STAGINGAPI
        protected string Url => AlertApp.CodeSettings.StagingAPI;
#else
        protected string Url => AlertApp.CodeSettings.ProductionAPI;
#endif




        protected readonly HttpClient _httpClient = new HttpClient();
        public BaseService()
        {
            _httpClient.BaseAddress = new Uri(Url);
        }
        protected static byte[] StreamToByteArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
