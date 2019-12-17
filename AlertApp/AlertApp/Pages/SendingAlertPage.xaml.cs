using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendingAlertPage : ContentPage
    {
        private bool DeletePressed;
        volatile int seconds = 10;
        volatile bool stop = false;
        Timer timer = null;
        string applicationPin;
        public SendingAlertPage(AlertType alertType)
        {

            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("alertType", alertType);
            this.BindingContext = ViewModelLocator.Instance.Resolve<SendingAlertPageViewModel>(parameters);

            pinLayout.Pin4Entry.TextChanged += Pin4_TextChanged;

            StartTimer();
            SetApplicationPin();

        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            Device.BeginInvokeOnMainThread(() =>
            {
                pinLayout.Pin1Entry.Focus();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void SetApplicationPin()
        {
            applicationPin = await ((ISendAlert)this.BindingContext).GetApplicationPin();

            pinLayout.Pin1Entry.Focus();

        }

        private async void StartTimer()
        {
            applicationPin = await ((ISendAlert)this.BindingContext).GetApplicationPin();
            for (int i = 0; i < seconds; i++)
            {
                if (!stop)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        alertLabel.Text = (seconds - i).ToString();//AppResources.SendingAlertIn + "\n" + (seconds - i);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                if (stop)
                    i = 11;
            }

            if (!stop)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    alertLabel.Text = "";//AppResources.SendingAlert;
                });

                ((ISendAlert)this.BindingContext).SendUserAlert();

            }
        }


        private async void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    string userInput = String.Format("{0}{1}{2}{3}", pinLayout.Pin1Entry.Text, pinLayout.Pin2Entry.Text, pinLayout.Pin3Entry.Text, pinLayout.Pin4Entry.Text);
                    if (userInput.Equals(applicationPin))
                    {
                        stop = true;
                        pinLayout.Pin4Entry.Unfocus();
                        try
                        {
                            await this.Navigation.PopAsync(false);
                        }
                        catch
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {                                
                                this.Navigation.PopModalAsync(false);                                
                            });

                        }

                    }
                }
            }
            catch (Exception)
            {

            }

        }

    }
}