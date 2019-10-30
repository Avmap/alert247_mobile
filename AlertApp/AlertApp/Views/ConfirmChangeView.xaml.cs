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

            Device.BeginInvokeOnMainThread(() =>
            {
                Pin1.Focus();
            });
        }
        
        private void Pin1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin2.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin3.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin4.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    // Pin4.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
            Task.Run(async () =>
            {
                var pin = await _localSettingsService.GetApplicationPin();
                if (userInput.Equals(pin))
                {
                    Confirmed = true;
                    await Navigation.PopModalAsync();
                }
            });

        }
    }
}