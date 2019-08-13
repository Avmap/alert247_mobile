using AlertApp.Model;
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
    public partial class DependandsPage : ContentPage
    {
        public DependandsPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<DependandsPageViewModel>();
        }

        protected override bool OnBackButtonPressed()
        {
            if (popupDependandsSettings.IsVisible)
            {
                popupDependandsSettings.IsVisible = false;
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var vm = this.BindingContext as DependandsPageViewModel;
            vm.ToggleSettings(e.SelectedItem as RadiusSetting, null);            
        }

        private void Button_OK_Dialog_Clicked(object sender, EventArgs e)
        {
            popupDependandsSettings.IsVisible = false;
        }

        private void More_Action_Click(object sender, EventArgs e)
        {
            popupDependandsSettings.IsVisible = true;
            radiusPicker.SelectedIndex = -1;
            datePicker.Date = DateTime.Now;
            datePicker.MinimumDate = DateTime.Now;
        }
    }
}