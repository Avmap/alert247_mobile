using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model.Api;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace AlertApp.Services.News
{
    class NewsService : BaseService, INewsService
    {
        public async Task<Response<NewsEntryResponse>> GetNews(string token)
        {
            var res = new Response<NewsEntryResponse>();
            try
            {
                var selectedLanguage = Preferences.Get(Utils.Settings.SelectedLanguage, "");
                //var json = JsonConvert.SerializeObject(new GetNewsPostBody { Token = token, Lang = selectedLanguage.Substring(0, 2) });

                var getNewsObj = new GetNewsPostBody { Token = token, Lang = selectedLanguage.Substring(0, 2) };

                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                //var response = await _httpClient.PostAsync("post/alert/getNews", content);

                var response = await _httpClient.GetAsync($"post/alert/getNews?api_key={getNewsObj.api_key}&lang={getNewsObj.Lang}");

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    //generic error handling here
                    res.ErrorCode = "Internal server error";
                    res.Status = "error";
                    return res;
                }
                if (response.Content != null)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    if (apiResponse != null)
                    {
                        return JsonConvert.DeserializeObject<Response<NewsEntryResponse>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorCode = ex.Message;
                res.Status = "error";
                res.IsOnline = false;
            }
            return res;
        }

        public async Task<Response<NewsEntryResponse>> GetNewsMock(string token)
        {
            var res = new Response<NewsEntryResponse>();
            res.Result = new NewsEntryResponse();
            Random rnd = new Random();
            for (var i = 0; i < 5; i++)
            {
                var x1 = new NewsEntry();
                if (i == 0)
                {
                    x1.Title = "Ανακοίνωση";
                    x1.Description = @"Σημαντικές πληροφορίες για το ταξίδι σας από/προς το Διεθνή Αερολιμένα Αθηνών&nbsp; θα βρείτε εδώ: <a href=""https://www.aia.gr/el/traveler/travellers-info/faq-for-covid19"">ΚΑΝΤΕ ΚΛΙΚ ΕΔΩ</a>. Για οποιαδήποτε πληροφορία σχετικά με το πρόγραμμα πτήσεων των αεροπορικών εταιρειών ή/ και συγκεκριμένες πτήσεις παρακαλούμε να επικοινωνείτε με την αεροπορική εταιρεία σας.<br>
                    Περισσότερες οδηγίες για ταξιδιώτες μπορείτε να δείτε εδώ: <a href=""https://eody.gov.gr/"" >https://eody.gov.gr/</a>.";
                    x1.Category = NewsEntryCategory.AIR;
                }
                else
                {
                    var j = rnd.Next(0, 5);
                    switch (j)
                    {
                        case 0:
                            x1.Category = NewsEntryCategory.AIR;
                            break;
                        case 1:
                            x1.Category = NewsEntryCategory.SEA;
                            break;
                        case 2:
                            x1.Category = NewsEntryCategory.TRANSPORT;
                            break;
                        default:
                            x1.Category = NewsEntryCategory.GENERIC;

                            break;
                    }
                    x1.Image = "http://alert247.gr/images/banner_participate.jpg";
                    x1.Title = "News Entry #" + i.ToString();
                    x1.Description = "Lorem ipsum description Lorem ipsum description ";
                }
                

                x1.PublishDate = DateTime.Now.ToString("d/M/yyyy HH:mm:ss");
                //res.Result.News = new List<NewsEntry>();
                res.Result.News.Add(x1);
            }
            
            return res;
        }
    }
}
