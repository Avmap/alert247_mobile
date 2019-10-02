using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Resx;
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
    public partial class ManageContactsPage : TabbedPage
    {
        int tabsAdded = 0;
        int tabsCount = 4;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        readonly IContacts _contacts;
        public int MyProperty { get; set; }
        public ManageContactsPage()
        {
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            _contacts = DependencyService.Get<IContacts>();            
            this.BindingContext = ViewModelLocator.Instance.Resolve<ManageContactsPageViewModel>();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RegisterForRefreshContacts();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnRegisterForRefreshContacts();
        }

        public void RegisterForRefreshContacts()
        {
            MessagingCenter.Subscribe<BaseViewModel, RefreshContactsEvent>(this, RefreshContactsEvent.Event, (sender, arg) =>
            {
                RefreshContacts();
            });
        }

        public void UnRegisterForRefreshContacts()
        {
            MessagingCenter.Unsubscribe<BaseViewModel, RefreshContactsEvent>(this, RefreshContactsEvent.Event);
        }

        

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
            tabsAdded++;
            if (tabsAdded == tabsCount)
            {
                RefreshContacts();
            }
        }

        public void RefreshContacts()
        {
            Task.Run(async () =>
            {
                //IF WE ADD NEW TAB WE MUST CHANGE tabsCount PROPERTY !!!!!!OnChildAdded!!!!!!!!!
                var vm = this.BindingContext as ManageContactsPageViewModel;
                var response = await vm.GetContacts();
                var addressBook = await ContactsHelp.GetAddressbook(_contacts,_contactProfileImageProvider);                               
                if (response != null)
                {
                    foreach (var item in this.Children)
                    {
                        if (item.BindingContext is IHaveContacts)
                        {
                            ((IHaveContacts)item.BindingContext).SetContacts(response,addressBook);
                        }
                    }
                }
                if (response.IsOnline == false) 
                {
                    await  DisplayAlert(AppResources.Warning, AppResources.NoInternetConnection, AppResources.OK);                    
                }
            });
        }

    }
}