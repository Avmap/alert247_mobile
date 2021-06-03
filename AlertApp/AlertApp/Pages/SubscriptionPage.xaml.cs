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
    public partial class SubscriptionPage : ContentPage
    {
        public SubscriptionPage()
        {
            BindingContext = this;
            IsBusy = true;
            InitializeComponent();
            URLSource = AlertApp.CodeSettings.SubscriptionURL;
        }

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

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            this.Indicator.IsVisible = false;
        }
    }
}