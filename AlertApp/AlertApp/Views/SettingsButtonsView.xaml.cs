﻿using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsButtonsView : ContentView
    {        
        public SettingsButtonsView ()
		{
			InitializeComponent ();
            SOS.IsToggled = Preferences.Get(Settings.SOS_BUTTON_VISIBLE, true);
            THREAT.IsToggled = Preferences.Get(Settings.THREAT_BUTTON_VISIBLE, true);
            FIRE.IsToggled = Preferences.Get(Settings.FIRE_BUTTON_VISIBLE, true);
            ACCIDENT.IsToggled = Preferences.Get(Settings.ACCIDENT_BUTTON_VISIBLE, true);
            CONTACTS.IsToggled = Preferences.Get(Settings.CONTACTS_BUTTON_VISIBLE, true); 
            INFORMATION.IsToggled = Preferences.Get(Settings.INFORMATION_BUTTON_VISIBLE, true); 
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            toggleSetting(args.Parameter as string, true);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchView = sender as Switch;
            toggleSetting(switchView.Id.ToString(),true);
        }

        private void toggleSetting(string setting,bool fromCode)
        {
            switch (setting)
            {
                case "SOS":
                    toggle(Settings.SOS_BUTTON_VISIBLE,SOS);
                    break;
                case "THREAT":
                    toggle(Settings.THREAT_BUTTON_VISIBLE, THREAT);
                    break;
                case "FIRE":
                    toggle(Settings.FIRE_BUTTON_VISIBLE, FIRE);
                    break;
                case "ACCIDENT":
                    toggle(Settings.ACCIDENT_BUTTON_VISIBLE, ACCIDENT);
                    break;
                case "CONTACTS":
                    toggle(Settings.CONTACTS_BUTTON_VISIBLE, CONTACTS);
                    break;
                case "INFORMATION":
                    toggle(Settings.INFORMATION_BUTTON_VISIBLE, INFORMATION);
                    break;

            }
        }

        private void toggle(string setting, Switch switchView)
        {
            var newValue = !switchView.IsToggled;            
            switchView.IsToggled = !switchView.IsToggled;
        }

        private void SaveChanges()
        {
            Preferences.Set(Settings.SOS_BUTTON_VISIBLE, SOS.IsToggled);
            Preferences.Set(Settings.THREAT_BUTTON_VISIBLE, THREAT.IsToggled);
            Preferences.Set(Settings.FIRE_BUTTON_VISIBLE, FIRE.IsToggled);
            Preferences.Set(Settings.ACCIDENT_BUTTON_VISIBLE, ACCIDENT.IsToggled);
            Preferences.Set(Settings.CONTACTS_BUTTON_VISIBLE, CONTACTS.IsToggled);
            Preferences.Set(Settings.INFORMATION_BUTTON_VISIBLE, INFORMATION.IsToggled);            
        }

        private void ConfirmSettingsClick(object sender, EventArgs e)
        {
            var confirmView = new ConfirmChangeView();            
            var page = new SettingContainerPage(AppResources.SettingButtonsTitle, AppResources.Confirmation, confirmView);
            page.Disappearing += (sender2, e2) =>
            {               
                if (confirmView.Confirmed){
                    SaveChanges();
                }
            }; 

            Navigation.PushModalAsync(page);
        }

       
    }
}