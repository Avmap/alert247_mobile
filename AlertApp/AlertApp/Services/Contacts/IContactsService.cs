using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Contacts
{
    public  interface IContactsService
    {
        Task<Response<GetContactsResponse>> GetContacts(string token);
        Task<Response<AddContactsResponse>> AddContacts(string token,string [] mobilephones);
        Task<Response<CheckContactsResponse>> CheckContacts(string token,string [] mobilephones);
    }
}
