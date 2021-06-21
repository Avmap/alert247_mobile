using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using AlertApp.Utils;
namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhatsNewPage : ContentPage
    {
        public WhatsNewPage(ContentPage next)
        {
            _next = next;
            BindingContext = this;
            IsBusy = true;
            InitializeComponent();
            
            URLSource = AlertApp.CodeSettings.WhatsNewURL.Replace("$LANG$", Preferences.Get(Settings.SelectedLanguage,"el").Split("-".ToCharArray())[0]);
        }
        private ContentPage _next;

        private string _urlSource;
        public string URLSource
        {
            get
            {
                return _urlSource;
            }
            set
            {
                _urlSource = value;
                OnPropertyChanged("URLSource");
            }
        }

        private void DoNotShowTapped(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new DialogSelectLanguage());
        }

        private void NextCommand(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(_next, Navigation.NavigationStack.First());
            Navigation.PopToRootAsync();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            toggleSetting(args.Parameter as string, true);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchView = sender as Switch;
            toggleSetting(switchView.Id.ToString(), true);
        }

        private void toggleSetting(string setting, bool fromCode)
        {
            switch (setting)
            {
                case "DoNotShow":
                    toggle(Settings.DoNotShowWhatsNew, DoNotShow);
                    break;
            }
        }

        private void toggle(string setting, Switch switchView)
        {
            var newValue = !switchView.IsToggled;
            //switchView.IsToggled = !switchView.IsToggled;
            Preferences.Set(Settings.DoNotShowWhatsNew, newValue);
        }
        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            
        }
    }
}