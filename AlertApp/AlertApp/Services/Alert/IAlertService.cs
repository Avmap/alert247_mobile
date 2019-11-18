using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Alert
{
    public interface IAlertService
    {
        Task<Response<SendAlertResponse>> SendAlert(string token,double? lat,double? lng,int type);
        Task<Response> AckAlert(string token, double? lat, double? lng, int type,int alertId,DateTime displayedTime,string publicKey);
    }
}
