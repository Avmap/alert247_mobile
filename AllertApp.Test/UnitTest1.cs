using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using AlertApp.ViewModels;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Setup()
        {
            var profileService = ViewModelLocator.Instance.Resolve<UserProfileService>();
            var cryptographyService = ViewModelLocator.Instance.Resolve<ICryptographyService>();
            string token = "eyJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1NTk4Mjg4MjAsIm5iZiI6MTU1OTgyODgyMCwiZXhwIjoxNTU5ODMyNDIwLCJpc3MiOiJBbGVydCBzZXJ2ZXIiLCJhdWQiOiJBbGVydCBtb2JpbGUiLCJ1c2VySUQiOiIxNSJ9.eJGZbL-qjRmWZxUZzRGm3PCR9xmpi_37tRD0Zf47q8o";
            var res = profileService.GetProfile(token, "15").Result;

            var profileData = cryptographyService.DecryptProfileData(res.Result.Profile).Result;


            Assert.IsNotNull(profileData);

            Assert.AreEqual(res.IsOk, true);
            Assert.IsNotNull(res.Result.Profile);
        }


    }
}