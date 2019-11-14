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
            StartTimer();
            SetApplicationPin();

        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            Device.BeginInvokeOnMainThread(() =>
            {
                Pin1.Focus();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void SetApplicationPin()
        {
            applicationPin = await ((ISendAlert)this.BindingContext).GetApplicationPin();
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




        private void Pin1_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin1.Text))
            {
                Pin1.Text = text;
                Pin2.Focus();
            }
            else
            {
                Pin2.Text = text;
                Pin2.Focus();
            }
        }

        private void Pin2_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin2.Text))
            {
                Pin2.Text = text;
                Pin3.Focus();
            }
            else
            {
                Pin3.Text = text;
                Pin3.Focus();
            }
        }

        private void Pin3_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin3.Text))
            {
                Pin3.Text = text;
                Pin4.Focus();
            }
            else
            {
                Pin4.Text = text;
                Pin4.Focus();
            }
        }

        private void Pin4_OnNumberEntered(string text)
        {

            if (string.IsNullOrWhiteSpace(Pin4.Text))
            {
                Pin4.Text = text;
            }
        }

        private void Pin1_OnDeleteButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            Pin1.Text = "";
        }

        private void Pin2_OnDeleteButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin2.Text) && Pin2.Text.Length == 1)
            {
                Pin2.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin2.Text))
            {
                Pin1.Focus();

            }
        }

        private void Pin3_OnDeleteButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin3.Text) && Pin3.Text.Length == 1)
            {
                Pin3.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin3.Text))
            {
                Pin2.Focus();
            }
        }

        private void Pin4_OnDeleteButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin4.Text) && Pin4.Text.Length == 1)
            {
                Pin4.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin4.Text))
            {
                Pin3.Focus();
            }
        }

        private void Pin_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                if (CheckFullPin())
                {
                    GetFocus(Pin4);
                    return;
                }

                var entry = sender as Entry;

                if (entry.Id == Pin1.Id)
                {
                    if (!string.IsNullOrWhiteSpace(Pin1.Text) && string.IsNullOrWhiteSpace(Pin2.Text) && !DeletePressed)
                    {
                        GetFocus(Pin2);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin2.Text) && string.IsNullOrWhiteSpace(Pin3.Text) && !DeletePressed)
                    {
                        GetFocus(Pin3);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin2.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(Pin2.Text) && string.IsNullOrWhiteSpace(Pin3.Text) && !DeletePressed)
                    {
                        GetFocus(Pin3);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin3.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin2.Text))
                    {
                        GetFocus(Pin2);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin4.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin2.Text))
                    {
                        GetFocus(Pin2);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin3.Text))
                    {
                        GetFocus(Pin3);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin4.Text))
                    {
                        GetFocus(Pin4);
                        return;
                    }
                }

            }
            catch (Exception)
            {

            }
            DeletePressed = false;
        }
        private bool CheckFullPin()
        {
            string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
            if (userInput.Length == 4)
            {
                return true;
            }
            return false;
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
                else
                {
                    Pin1.Focus();
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
                else
                {
                    Pin2.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private async void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
                    if (userInput.Equals(applicationPin))
                    {
                        stop = true;
                        await this.Navigation.PopAsync();
                    }
                }
                else
                {
                    Pin3.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void GetFocus(Entry entry)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                entry.Focus();
            });
        }
    }
}