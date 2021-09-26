using AlertApp.Infrastructure;
using AlertApp.Resx;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EnterMobileNumberPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            txtPhone.Focus();
        }

        public EnterMobileNumberPage()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<EnterMobileNumberPageViewModel>();
           

        }

        private async void OnButtonNextClicked(object sender, EventArgs e)
        {
            var vm = this.BindingContext as EnterMobileNumberPageViewModel;

            if (!string.IsNullOrWhiteSpace(vm.Mobile))
            {
                string message = AppResources.SmsVerificationMessage + " " + String.Format("{0}{1}", vm.CountryPrefix, vm.Mobile) + ". " + AppResources.InformUserPhoneNumber;

                var confirm = await DisplayAlert(AppResources.Verification, message, AppResources.ContinueDialogButton, AppResources.Cancel);
                if (confirm)
                {
                    await Navigation.PushAsync(new EnterActivationCodePage(String.Format("{0}{1}", vm.CountryPrefix, vm.Mobile)), false);
                }
            }
            else
            {
                await DisplayAlert(AppResources.Warning, AppResources.WarningFillNumber, "OK");                
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           // Task.Run(async () =>
           // {
                 Navigation.PopAsync(false);
           // });
           
        }

        private void ContinueCommand(object sender, EventArgs e)
        {

        }
    }
}