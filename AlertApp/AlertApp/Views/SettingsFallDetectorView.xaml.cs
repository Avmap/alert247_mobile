using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsFallDetectorView : ContentView
    {
        readonly INotificationManager _notificationManager;
        public SettingsFallDetectorView()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsFallDetectorViewModel>();
            _notificationManager = DependencyService.Get<INotificationManager>();
        }

        private void ConfirmSettingsClick(object sender, EventArgs e)
        {
            var vm = this.BindingContext as SettingsFallDetectorViewModel;
            var isValid = vm.IsValid();
            if (!isValid)
            {
                _notificationManager.ToastNotification(AppResources.FillAllDetectorValues);
                return;
            }

            // var confirmView = new ConfirmChangeView();
            // var page = new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.Confirmation, confirmView);
            // page.Disappearing += (sender2, e2) =>
            // {
            //     if (confirmView.Confirmed)
            //     {
            //         SaveChanges();
            //     }
            // };
            
            SaveChanges();

            //Navigation.PushModalAsync(page);
        }

        private void SaveChanges()
        {
            var vm = this.BindingContext as SettingsFallDetectorViewModel;
            vm.SaveSettings();
            Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
        }
    }
}