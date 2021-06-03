using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AlertApp.Services.News
{
    public interface INewsService
    {
        Task<Response<NewsEntryResponse>> GetNews(string token);

        Task<Response<NewsEntryResponse>> GetNewsMock(string token);
    }
}
