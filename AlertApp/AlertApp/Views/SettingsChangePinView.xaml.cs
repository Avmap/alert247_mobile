using AlertApp.Resx;
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
        SettingsChangePinViewModel vm;
        public SettingsChangePinView()
        {
            InitializeComponent();

            vm = ViewModelLocator.Instance.Resolve<SettingsChangePinViewModel>();
            this.BindingContext = vm;
            Task.Run(async () =>
              {
                  _ApplicationPin = await ((SettingsChangePinViewModel)this.BindingContext).GetApplicationPin();
              });


            pinLayout.Pin4Entry.TextChanged += Pin4_TextChanged;

            Device.BeginInvokeOnMainThread(() =>
            {
                pinLayout.Pin1Entry.Focus();
            });


            newPinLayout.Pin4Entry.TextChanged += PinTextChanged;
            vePinLayout.Pin4Entry.TextChanged += PinTextChanged;

            vePinLayout.Pin1Entry.SetBinding(Entry.TextProperty, nameof(vm.VePin1));
            vePinLayout.Pin2Entry.SetBinding(Entry.TextProperty, nameof(vm.VePin2));
            vePinLayout.Pin3Entry.SetBinding(Entry.TextProperty, nameof(vm.VePin3));
            vePinLayout.Pin4Entry.SetBinding(Entry.TextProperty, nameof(vm.VePin4));
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
                CheckNewPin();
            }
            catch (Exception)
            {

            }

        }

        private void CheckPin()
        {
            var vm = this.BindingContext as SettingsChangePinViewModel;
            string userInput = String.Format("{0}{1}{2}{3}", pinLayout.Pin1Entry.Text, pinLayout.Pin2Entry.Text, pinLayout.Pin3Entry.Text, pinLayout.Pin4Entry.Text);
            if (!string.IsNullOrWhiteSpace(userInput) && userInput.Equals(_ApplicationPin))
            {

                vm.NewPinLayoutVisible = true;
                newPinLayout.Pin1Entry.Focus();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert(AppResources.Warning, AppResources.WrongPin, "OK");
                vm.NewPinLayoutVisible = false;             
            }
        }

        private void CheckNewPin()
        {
            var vm = this.BindingContext as SettingsChangePinViewModel;
            string newPin = String.Format("{0}{1}{2}{3}", newPinLayout.Pin1Entry.Text, newPinLayout.Pin2Entry.Text, newPinLayout.Pin3Entry.Text, newPinLayout.Pin4Entry.Text);
            string newPinVerification = String.Format("{0}{1}{2}{3}", vePinLayout.Pin1Entry.Text, vePinLayout.Pin2Entry.Text, vePinLayout.Pin3Entry.Text, vePinLayout.Pin4Entry.Text);
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