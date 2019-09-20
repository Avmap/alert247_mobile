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

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCommunityPage : ContentPage
    {     
        public MyCommunityPage()
        {
            InitializeComponent();
            bottomSheet.IsVisible = false;
            bottomSheet.TranslationY = DeviceDisplay.MainDisplayInfo.Height;
            this.BindingContext = ViewModelLocator.Instance.Resolve<MyCommunityPageViewModel>();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
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
        private void ShadowClicked(object sender, EventArgs e)
        {
            ShowBottomSheet();
        }        
        protected override bool OnBackButtonPressed()
        {
            if (bottomSheet.IsVisible)
            {
                ShowBottomSheet();
                return true;
            }

            return base.OnBackButtonPressed();
        }

      
        
    }
}