using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.Utils;
using AlertApp.ViewModels;
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
    public partial class SelectLanguagePage : ContentPage
    {
        public SelectLanguagePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = Utils.ViewModelProvider.SelectLanguageViewModel();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new DialogSelectLanguage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lang = Preferences.Get(Settings.SelectedLanguage, "");
            if (!string.IsNullOrWhiteSpace(lang))
            {
                CultureInfo culture = new CultureInfo(lang);
                Resx.AppResources.Culture = culture;
            }
            lblSelectLanguage.Text = AppResources.SelectLanguage;
            lblSelectLanguageMessage.Text = AppResources.SelectLanguageMessage;
            btnContinue.Text = AppResources.Continue;
        }

    }
}