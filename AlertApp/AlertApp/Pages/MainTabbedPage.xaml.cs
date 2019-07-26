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
    public partial class MainTabbedPage : TabbedPage
    {

        public MainTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<MainTabbedPageViewModel>();
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "manage_contact_primary.png",
                Command = ((MainTabbedPageViewModel)this.BindingContext).OpenContactsScreen,
            });
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}