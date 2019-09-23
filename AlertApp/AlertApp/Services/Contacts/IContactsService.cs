﻿using AlertApp.Model.Api;
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
        Task<Response<bool>> AcceptAdd(string token, string mobilephone);
        Task<Response<bool>> IgnoreAdd(string token, string mobilephone);
        Task<Response<bool>> BlockAdd(string token, string mobilephone);
    }
}
