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
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<EnterApplicationPinCodePageViewModel>();       
        }
     
       protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}