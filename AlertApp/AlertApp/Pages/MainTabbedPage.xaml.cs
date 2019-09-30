using AlertApp.Infrastructure;
using AlertApp.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage() : this(null)
        {

        }

        public MainTabbedPage(NotificationAction notificationAction)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<MainTabbedPageViewModel>();
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "manage_contact_primary.png",
                Command = ((MainTabbedPageViewModel)this.BindingContext).OpenContactsScreen,
            });

            if (notificationAction != null)
            {
                switch (notificationAction.Type)
                {
                    case (int)NotificationAction.ActionType.Sos:
                    //    navigate(notificationAction);
                        break;
                }
            }            
        }

    
        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
    }
}