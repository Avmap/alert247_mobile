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
    public partial class SettingsAccountHistoryView : ContentView
    {
        public SettingsAccountHistoryView()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsAccountHistoryViewModel>();
        }
    }
}