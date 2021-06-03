using AlertApp.Model.Api;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhoAlertsMePage : ContentView
    {
        private AlertApp.Model.Api.Contact ClickedContact { get; set; }
        public WhoAlertsMePage()
        {
            InitializeComponent();
            bottomSheet.IsVisible = false;
            bottomSheet.TranslationY = DeviceDisplay.MainDisplayInfo.Height;
            this.BindingContext = ViewModelLocator.Instance.Resolve<WhoAlertsMePageViewModel>();
        }

        private void ShadowClicked(object sender, EventArgs e)
        {
            ShowBottomSheet();
        }

        private async void ShowBottomSheet()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            //if (popupLoginView.TranslationY == -mainDisplayInfo.Height)
            if (bottomSheet.TranslationY == mainDisplayInfo.Height)
            {
                shadow.IsVisible = true;
                bottomSheet.IsVisible = true;
                //  popupLoginView.TranslationY = 0;
                await bottomSheet.TranslateTo(0, 0, 100);//, Easing.BounceIn
            }
            else
            {
                shadow.IsVisible = false;
                await bottomSheet.TranslateTo(bottomSheet.TranslationX, mainDisplayInfo.Height, 100);
                //popupLoginView.TranslationY = -mainDisplayInfo.Height;
                bottomSheet.IsVisible = false;
            }
        }
        //protected override bool OnBackButtonPressed()
        //{
        //    if (bottomSheet.IsVisible)
        //    {
        //        ShowBottomSheet();
        //        return true;
        //    }

        //    return base.OnBackButtonPressed();
        //}


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            if (args != null)
            {
                var vm = this.BindingContext as WhoAlertsMePageViewModel;
                if (!vm.Busy)
                {
                    ClickedContact = args.Parameter as AlertApp.Model.Api.Contact;

                    if (ClickedContact.Accepted)
                    {
                        //vm.NavigateToAcceptCommunityReqeustScreen(ClickedContact);
                        ShowBottomSheet();
                    }
                    else
                    {
                        vm.NavigateToAcceptCommunityReqeustScreen(ClickedContact);
                    }

                }
            }
        }

        private void OnRemoveUserClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as WhoAlertsMePageViewModel;
            Task.Run(async () =>
            {
                await vm.RemoveUser(ClickedContact);
            });
            ShowBottomSheet();
        }
        private void OnBlockUserClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as WhoAlertsMePageViewModel;
            Task.Run(async () =>
            {
                await vm.BlockUser(ClickedContact);
            });
            ShowBottomSheet();
        }

    }
}