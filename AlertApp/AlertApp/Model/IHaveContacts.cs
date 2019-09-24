using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.Model
{
    public interface IHaveContacts
    {
        void SetContacts(Response<Api.GetContactsResponse> response, List<ImportContact> addressBook);
    }
}
