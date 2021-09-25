using AlertApp.MessageCenter;
using AlertApp.Model.Api;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlertApp.Resx;


namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCommunityPage : ContentView
    {
        private AlertApp.Model.Api.Contact ClickedContact { get; set; }
        public MyCommunityPage()
        {
            InitializeComponent();
            
            this.BindingContext = ViewModelLocator.Instance.Resolve<MyCommunityPageViewModel>();
        }


        private async void ShowBottomSheet()
        {
            var p = this.Parent.Parent.Parent;
            var x = (ContentPage)p;
            var choice = await x.DisplayActionSheet(String.IsNullOrEmpty(ClickedContact.FullName)? ClickedContact.Cellphone : ClickedContact.FullName, AppResources.Cancel, AppResources.RemoveUser, AppResources.RemoveUserAnalytical);

            if (choice.Equals(AppResources.RemoveUser) || choice.Equals(AppResources.RemoveUserAnalytical))
            {
                OnRemoveUserClick(null, null);
            }
            else if (choice.Equals(AppResources.RemoveUser))
            {
                OnBlockUserClick(null, null);
            }

            //switch (choice)
            //{
            //    case nameof(AppResources.RemoveUser):
            //    case nameof(AppResources.RemoveUserAnalytical):
            //        OnRemoveUserClick(null,null);
            //        break;
            //    case nameof(AppResources.BlockUser):
            //        OnBlockUserClick(null, null);
            //        break;
            //}
            //var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            ////if (popupLoginView.TranslationY == -mainDisplayInfo.Height)
            //if (bottomSheet.TranslationY == mainDisplayInfo.Height)
            //{
            //    shadow.IsVisible = true;
            //    bottomSheet.IsVisible = true;
            //    //  popupLoginView.TranslationY = 0;
            //    await bottomSheet.TranslateTo(0, 0, 100);//, Easing.BounceIn
            //}
            //else
            //{
            //    shadow.IsVisible = false;
            //    await bottomSheet.TranslateTo(bottomSheet.TranslationX, mainDisplayInfo.Height, 100);
            //    //popupLoginView.TranslationY = -mainDisplayInfo.Height;
            //    bottomSheet.IsVisible = false;
            //}
        }
        private void ShadowClicked(object sender, EventArgs e)
        {
            ShowBottomSheet();
        }

        private void OnRemoveUserClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as MyCommunityPageViewModel;
            Task.Run(async () =>
            {
                await vm.RemoveUser(ClickedContact);
            });
            //ShowBottomSheet();
        }

        private void OnBlockUserClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as MyCommunityPageViewModel;
            Task.Run(async () =>
            {
                await vm.BlockUser(ClickedContact);
            });
            //ShowBottomSheet();
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
                var vm = this.BindingContext as MyCommunityPageViewModel;
                if (!vm.Busy)
                {
                    ClickedContact = args.Parameter as AlertApp.Model.Api.Contact;
                    ShowBottomSheet();
                }
            }

        }
    }
}