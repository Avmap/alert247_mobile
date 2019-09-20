using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    { 
        public MainPage()
        {
            InitializeComponent();            
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<MainPageViewModel>();
        }
        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
    }
}