using AlertApp.Resx;
using AlertApp.Utils;
using AlertApp.ViewModels;
using AlertApp.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        bool codeTrigger = false;
        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
            //guardianSwitch.On = ((SettingsPageViewModel)this.BindingContext).AllwaysOn;
        }

      
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            ToggleGuardianSwitch(e.Value, false);
        }
        private void GuardianSwitch_Tapped(object sender, EventArgs e)
        {
            var switchCell = sender as SwitchCell;
            ToggleGuardianSwitch(!switchCell.On, true);
        }

        private async void ToggleGuardianSwitch(bool value, bool fromCell)
        {
            var vm = this.BindingContext as SettingsPageViewModel;
            if (value != vm.AllwaysOn && !codeTrigger)
            {
                if (value)
                {
                    var result = await vm.EnableGuardian();
                    if (!result.Ok)
                    {
                        codeTrigger = true;
                       // guardianSwitch.On = false;
                        codeTrigger = false;
                    }
                    else if (result.Ok && fromCell)
                    {
                        codeTrigger = true;
                       // guardianSwitch.On = true;
                        codeTrigger = false;
                    }
                }
                else
                {
                    vm.DisableGuardian();
                    if (fromCell)
                    {
                        codeTrigger = true;
                      //  guardianSwitch.On = false;
                        codeTrigger = false;
                    }
                }
            }

        }

        private void Language_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingLanguageTitle, AppResources.SettingLanguageSubTitle, new SelectLanguageView()), false);
        }
        private void ButtonsView_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingButtonsTitle, AppResources.SettingButtonsSubTitle, new SettingsButtonsView()), false);
        }

        private void Location_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.SettingPermissionLocationSubTitle, new SettingsLocationView()), false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((SettingsPageViewModel)this.BindingContext).HideShowButtons();
        }
    }
}