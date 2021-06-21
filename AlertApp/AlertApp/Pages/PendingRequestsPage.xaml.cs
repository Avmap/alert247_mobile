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
using AlertApp.Resx;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingRequestsPage : ContentView
    {
        private AlertApp.Model.Api.Contact ClickedContact { get; set; }
        public PendingRequestsPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<PendingRequestsPageViewModel>();
        }

        private void ShadowClicked(object sender, EventArgs e)
        {
            ShowBottomSheet();
        }

        private async void ShowBottomSheet()
        {
            var p = this.Parent.Parent.Parent;
            var x = (ContentPage)p;
            var choice = await x.DisplayActionSheet(String.IsNullOrEmpty(ClickedContact.FullName) ? ClickedContact.Cellphone : ClickedContact.FullName, AppResources.Cancel, null, AppResources.RemoveUser, AppResources.BlockUser);
            switch (choice)
            {
                case nameof(AppResources.RemoveUser):
                    OnRemoveUserClick(null, null);
                    break;
                case nameof(AppResources.BlockUser):
                    OnBlockUserClick(null, null);
                    break;
            }
        }
       

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            if (args != null)
            {
                var vm = this.BindingContext as PendingRequestsPageViewModel;
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
            var vm = this.BindingContext as PendingRequestsPageViewModel;
            Task.Run(async () =>
            {
                await vm.RemoveUser(ClickedContact);
            });
            ShowBottomSheet();
        }
        private void OnBlockUserClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as PendingRequestsPageViewModel;
            Task.Run(async () =>
            {
                await vm.BlockUser(ClickedContact);
            });
            ShowBottomSheet();
        }

    }
}