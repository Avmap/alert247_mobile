using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model;
using AlertApp.Model.Api;

namespace AlertApp.Services.Registration
{
    public class FakeRegistrationService : IRegistrationService
    {
        public Task<Response<ConfirmRegistrationResponse>> ConfirmRegistration(string cellphone, string otpVerifcationCode)
        {
            throw new NotImplementedException();
        }

        public async Task<RegistrationField[]> GetRegistrationFields(string language)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var result = new List<RegistrationField>();

            //result.Add(new RegistrationField
            //{
            //    Field = "name",
            //    Label = language.Equals("el") ? "Ονομα" : "Name",
            //    DataType = 1,
            //});
            //result.Add(new RegistrationField
            //{
            //    Field = "lastname",
            //    Label = language.Equals("el") ? "Επώνυμο" : "Last Name",
            //    DataType = 1,
            //});
            //result.Add(new RegistrationField
            //{
            //    Field = "birth",
            //    Label = language.Equals("el") ? "Ημ/νία γέννησης" : "Date of Birth",
            //    DataType = 2,
            //});

            return result.ToArray();
        }

        public Task<Response> OtpRequest(string cellphone)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Register(RegisterBody registerBody)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Register(string cellphone, string language)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Register(string cellphone, string language, string applicationHash)
        {
            throw new NotImplementedException();
        }
    }
}
