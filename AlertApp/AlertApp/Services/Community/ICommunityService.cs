using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Community
{
    public interface ICommunityService
    {
        Task<Response<CommunityResponse>> GetCommunity(string userid);
        Task<Response<DependantsResponse>> GetDependands(string userid);
    }
}
