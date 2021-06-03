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
            MapSource = AlertApp.CodeSettings.MapURL;
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