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
	public partial class SettingsPage : ContentPage
	{
        bool codeTrigger = false;
		public SettingsPage ()
		{
			InitializeComponent ();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
            guardianSwitch.On = ((SettingsPageViewModel)this.BindingContext).AllwaysOn;
        }

        private async void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
        {
            var vm = this.BindingContext as SettingsPageViewModel;
            if (e.Value != vm.AllwaysOn && !codeTrigger)
            {
                if (e.Value)
                {
                    var result = await vm.EnableGuardian();
                    if (!result.Ok)
                    {
                        codeTrigger = true;
                        guardianSwitch.On = false;
                        codeTrigger = false;
                    }
                }
                else
                {
                    vm.DisableGuardian();
                }
            }

        }
    }
}