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
            if (Device.RuntimePlatform == Device.iOS)
            {
            //    ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            }
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<MainPageViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MainPageViewModel)this.BindingContext).HideShowButtons();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
    }
}