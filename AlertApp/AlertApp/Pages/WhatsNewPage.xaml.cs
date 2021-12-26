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

            var selectedLanguage = Preferences.Get(Settings.SelectedLanguage, "el");
            selectedLanguage = selectedLanguage.Substring(0, 2);

            URLSource = CodeSettings.WhatsNewURL.Replace("$LANG$", selectedLanguage);
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
            if(Navigation.NavigationStack.Count > 0){
                Navigation.InsertPageBefore(_next, Navigation.NavigationStack.First());
            }else
            {
                var existingPages = _next.Navigation.NavigationStack.ToList();
                foreach (var page in existingPages)
                {
                    Navigation.RemovePage(page);
                }
                _next.Parent = null;
                Navigation.PushAsync(_next);
            }
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
            toggle(Settings.DoNotShowWhatsNew, switchView);

            //Property Id is GUID here.
            //toggleSetting(switchView.Id.ToString(), true);
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
            //var newValue = !switchView.IsToggled;
            //switchView.IsToggled = !switchView.IsToggled;
            Preferences.Set(setting, switchView.IsToggled);
        }

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            
        }
    }
}