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
    public partial class SettingsChangePinView : ContentView
    {
        public string _ApplicationPin { get; set; }
        public SettingsChangePinView()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsChangePinViewModel>();
            Task.Run(async () =>
            {
                _ApplicationPin = await ((SettingsChangePinViewModel)this.BindingContext).GetApplicationPin();
            });
        }

        private void Pin1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin2.Focus();
                    CheckPin();
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
                    CheckPin();
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
                    CheckPin();
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
                    CheckPin();
                }
            }
            catch (Exception)
            {

            }

        }

        private void PinTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    var entry = sender as Entry;
                    if (entry.Id == NewPin1.Id)
                    {
                        NewPin2.Focus();
                    }
                    if (entry.Id == NewPin2.Id)
                    {
                        NewPin3.Focus();
                    }
                    if (entry.Id == NewPin3.Id)
                    {
                        NewPin4.Focus();
                    }

                    if (entry.Id == VePin1.Id)
                    {
                        VePin2.Focus();
                    }
                    if (entry.Id == VePin2.Id)
                    {
                        VePin3.Focus();
                    }
                    if (entry.Id == VePin3.Id)
                    {
                        VePin4.Focus();
                    }
                }

                CheckNewPin();
            }
            catch (Exception)
            {

            }

        }

        private void CheckPin()
        {
            var vm = this.BindingContext as SettingsChangePinViewModel;
            string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
            if (!string.IsNullOrWhiteSpace(userInput) && userInput.Equals(_ApplicationPin))
            {

                vm.NewPinLayoutVisible = true;
                NewPin1.Focus();
            }
            else
            {
                vm.NewPinLayoutVisible = false;
            }
        }

        private void CheckNewPin()
        {
            var vm = this.BindingContext as SettingsChangePinViewModel;
            string newPin = String.Format("{0}{1}{2}{3}", NewPin1.Text, NewPin2.Text, NewPin3.Text, NewPin4.Text);
            string newPinVerification = String.Format("{0}{1}{2}{3}", VePin1.Text, VePin2.Text, VePin3.Text, VePin4.Text);
            if (!string.IsNullOrWhiteSpace(newPin) && !string.IsNullOrWhiteSpace(newPinVerification) && newPin.Equals(newPinVerification))
            {

                vm.CanFinish = true;

            }
            else
            {
                vm.CanFinish = false;
            }
        }
    }
}