using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model;
using AlertApp.Model.Api;

namespace AlertApp.Services.Registration
{
    public interface IRegistrationService
    {
        Task<Response> Register(string cellphone, string language, string applicationHash);

        Task<Response<ConfirmRegistrationResponse>> ConfirmRegistration(string cellphone, string otpVerifcationCode);     
        Task<Response> OtpRequest(string cellphone);

        Task<Response<ConfirmRegistrationResponse>> GetRegistrationFields(string token);
    }
}
