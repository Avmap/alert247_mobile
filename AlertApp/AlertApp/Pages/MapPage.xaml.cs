using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            BindingContext = this;
            IsBusy = true;
            InitializeComponent();

            var selectedLanguage = Preferences.Get(Utils.Settings.SelectedLanguage, "");
            selectedLanguage = selectedLanguage.Substring(0, 2);

            switch (selectedLanguage)
            {
                case "el":
                    MapSource = AlertApp.CodeSettings.MapURL;
                    break;
                case "en":
                    MapSource = AlertApp.CodeSettings.MapURLEn;
                    break;
                case "bg":
                    MapSource = AlertApp.CodeSettings.MapURLBg;
                    break;
                case "zh":
                    MapSource = AlertApp.CodeSettings.MapURLZh;
                    break;
                case "fr":
                    MapSource = AlertApp.CodeSettings.MapURLFr;
                    break;
                case "de":
                    MapSource = AlertApp.CodeSettings.MapURLDe;
                    break;
                case "it":
                    MapSource = AlertApp.CodeSettings.MapURLIt;
                    break;
                case "ru":
                    MapSource = AlertApp.CodeSettings.MapURLRu;
                    break;
                default:
                    MapSource = AlertApp.CodeSettings.MapURL;
                    break;
            }
        }
        private string _mapSource;
        public string MapSource
        {
            get
            {
                return _mapSource;
            }
            set
            {
                _mapSource = value;
                OnPropertyChanged("MapSource");
            }
        }



        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            this.Indicator.IsVisible = false;
        }
    }
}