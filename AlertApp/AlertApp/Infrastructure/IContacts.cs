using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Infrastructure
{
    public interface IContacts
    {
        Contact GetContactDetails(string cellphone);
    }
}
