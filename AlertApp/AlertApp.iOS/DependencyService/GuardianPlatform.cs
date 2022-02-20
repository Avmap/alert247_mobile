using AlertApp.Infrastructure;
using AlertApp.iOS.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(GuardianPlatform))]
namespace AlertApp.iOS.DependencyService
{
    public class GuardianPlatform : IGuardian
    {
        public void StartGuardianService()
        {
            //throw new System.NotImplementedException();
        }

        public void StopGuardianService()
        {
            //throw new System.NotImplementedException();
        }

        public void StartDetector()
        {
            //throw new System.NotImplementedException();
        }
    }
}