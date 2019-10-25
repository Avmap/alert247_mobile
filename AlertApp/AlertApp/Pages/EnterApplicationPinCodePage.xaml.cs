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
	public partial class EnterApplicationPinCodePage : ContentPage
	{   
		public EnterApplicationPinCodePage()
		{
			InitializeComponent ();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<EnterApplicationPinCodePageViewModel>();
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
       
    }
}