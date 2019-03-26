using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;

namespace AlertApp.Services.Community
{
    public class CommunityService : ICommunityService
    {
        public async Task<Response<CommunityResponse>> GetCommunity(string userid)
        {
            var result = new Response<CommunityResponse>();
            result.Status = "ok";
            result.Result = new CommunityResponse
            {
                MyCommunity = new Contact[4]
            {
                new Contact { FirstName = "Joseph",LastName = "Hudson"},
                new Contact { FirstName = "Logan",LastName = "Stewart"},
                new Contact { FirstName = "Isaac",LastName = "Gill"},
                new Contact { FirstName = "Charlie",LastName = "Jackson"}
            }
            };
            return result;
        }

        public async Task<Response<DependantsResponse>> GetDependands(string userid)
        {
            var result = new Response<DependantsResponse>();
            result.Status = "ok";
            result.Result = new DependantsResponse
            {
                Dependants = new Contact[4]
            {
                 new Contact { FirstName = "Joseph",LastName = "Hudson"},
                new Contact { FirstName = "Logan",LastName = "Stewart"},
                new Contact { FirstName = "Isaac",LastName = "Gill"},
                new Contact { FirstName = "Charlie",LastName = "Jackson"}
            }
            };
            return result;
        }
    }
}
