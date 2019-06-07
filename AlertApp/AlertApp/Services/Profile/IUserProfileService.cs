using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Profile
{
    public interface IUserProfileService
    {
        Task<Response> StoreProfile(Dictionary<string, string> registrationValues, string token, string publicKey);
        Task<Response> Ping(string token, double? lat, double? lng,string firebaseToken);
        Task<Response<GetProfileResponse>> GetProfile(string token, string userid);

        
    }
}
