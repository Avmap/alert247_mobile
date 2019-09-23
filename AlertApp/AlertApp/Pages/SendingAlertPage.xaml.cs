﻿using AlertApp.Infrastructure;
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
                
        volatile int timerRunTimes = 11;
        volatile bool stop = false;
        Timer timer = null;
        string applicationPin;
        string sendingAlert;
        public SendingAlertPage(AlertType alertType)
        {

            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("alertType", AlertType.UserAlert);
            this.BindingContext = ViewModelLocator.Instance.Resolve<SendingAlertPageViewModel>(parameters);
            sendingAlert = AppResources.SendingAlertIn + " ";           
            StartTimer();
            SetApplicationPin();            
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
            for (int i = 0; i < 11; i++)
            {
                if (!stop)
                {
                    Debug.WriteLine("i = " + i + "  " + (60 - (i * 5)));
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        countdownStack.Children.Add(new Label
                        {
                            Text = sendingAlert + (60 - (i * 5)) + "\"",
                            TextColor = Color.Black,
                            FontSize = 22,
                            HorizontalTextAlignment = TextAlignment.Center
                        });
                        scrollView.ScrollToAsync(0, scrollView.Height, false);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
                if (stop)
                    i = 11;
            }

            if (!stop)
            {
                ((ISendAlert)this.BindingContext).SendUserAlert();
            }
        }
    
        private async void PinEntry_TextChanged(object sender, TextChangedEventArgs e)
        {            
            if (!string.IsNullOrWhiteSpace(e.NewTextValue) && e.NewTextValue.Equals(applicationPin))
            {
                stop = true;                
                await this.Navigation.PopAsync();
            }
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
        private async void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
                if (!string.IsNullOrWhiteSpace(e.NewTextValue) && userInput.Equals(applicationPin))
                {
                    stop = true;
                    await this.Navigation.PopAsync();
                }
            }
            catch (Exception)
            {

            }

        }
    }
}