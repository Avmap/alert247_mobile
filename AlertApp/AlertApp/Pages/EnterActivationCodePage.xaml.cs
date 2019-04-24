using AlertApp.Infrastructure;
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
    public partial class EnterActivationCodePage : ContentPage
    {
        public EnterActivationCodePage(string mobilenumber)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = Utils.ViewModelProvider.EnterActivationCodeViewModel(mobilenumber);
        }


        private void Verification1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Verification2.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void Verification2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Verification3.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void Verification3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Verification4.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void Verification4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Verification5.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void Verification5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Verification6.Focus();
                }
            }
            catch (Exception)
            {

            }

        }

        private void Verification6_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    var vm = this.BindingContext as EnterActivationCodePageModel;
                    if (vm != null)
                    {
                        vm.ContinueCommand.Execute(null);
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (this.BindingContext != null && this.BindingContext is EnterActivationCodePageModel)
            {
                var vm = this.BindingContext as EnterActivationCodePageModel;
                vm.RegisterForSmsEvent();

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (this.BindingContext != null && this.BindingContext is EnterActivationCodePageModel)
            {
                var vm = this.BindingContext as EnterActivationCodePageModel;
                vm.UnRegisterForSmsEvent();
            }
        }
    }
}