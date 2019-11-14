using AlertApp.Resx;
using AlertApp.Services.Settings;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmChangeView : ContentView
    {
        private readonly ILocalSettingsService _localSettingsService;
        public bool Confirmed { get; set; }
        public ConfirmChangeView()
        {
            InitializeComponent();
            _localSettingsService = ViewModelLocator.Instance.Resolve<ILocalSettingsService>();
        }       

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //var vm  = 
            string userInput = String.Format("{0}{1}{2}{3}", pinLayout.Pin1Entry.Text, pinLayout.Pin2Entry.Text, pinLayout.Pin3Entry.Text, pinLayout.Pin4Entry.Text);
            Task.Run(async () =>
            {
                var pin = await _localSettingsService.GetApplicationPin();
                if (userInput.Equals(pin))
                {
                    Confirmed = true;
                    await Navigation.PopModalAsync();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(userInput))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                                           {
                                               Application.Current.MainPage.DisplayAlert(AppResources.Warning, AppResources.WrongPin, "OK");
                                               pinLayout.Pin1Entry.Focus();
                                           });
                    }

                }
            });

        }
    }
}