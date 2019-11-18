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
    public partial class ManageContactsPage : ContentPage
    {
        int tabsAdded = 0;
        int tabsCount = 4;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        readonly IContacts _contacts;
        public int MyProperty { get; set; }
        public ManageContactsPage()
        {
            InitializeComponent();
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            _contacts = DependencyService.Get<IContacts>();
            this.BindingContext = ViewModelLocator.Instance.Resolve<ManageContactsPageViewModel>();
           
            this.container.ChildAdded += Container_ChildAdded;

            var communityPage = new MyCommunityPage();
            var dependantsPage = new DependandsPage();
            var whoalertMePage = new WhoAlertsMePage();
            var blockedUsersPage = new BlockedUsersPage();


            AbsoluteLayout.SetLayoutBounds(communityPage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(communityPage, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(dependantsPage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(dependantsPage, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(whoalertMePage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(whoalertMePage, AbsoluteLayoutFlags.All);

            AbsoluteLayout.SetLayoutBounds(blockedUsersPage, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(blockedUsersPage, AbsoluteLayoutFlags.All);

            this.container.Children.Add(communityPage);
            this.container.Children.Add(dependantsPage);
            this.container.Children.Add(whoalertMePage);
            this.container.Children.Add(blockedUsersPage);

            dependantsPage.IsVisible = false;
            whoalertMePage.IsVisible = false;
            blockedUsersPage.IsVisible = false;

            
        }

        private void Container_ChildAdded(object sender, ElementEventArgs e)
        {
            tabsAdded++;
            if (tabsAdded == tabsCount)
            {
                RefreshContacts();
            }
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

        public void RefreshContacts()
        {
            Task.Run(async () =>
            {
                //IF WE ADD NEW TAB WE MUST CHANGE tabsCount PROPERTY !!!!!!OnChildAdded!!!!!!!!!
                var vm = this.BindingContext as ManageContactsPageViewModel;
                var response = await vm.GetContacts();
                var addressBook = await ContactsHelp.GetAddressbook(_contacts, _contactProfileImageProvider);
                if (response != null)
                {
                    foreach (var item in this.container.Children)
                    {
                        if (item.BindingContext is IHaveContacts)
                        {
                            ((IHaveContacts)item.BindingContext).SetContacts(response, addressBook);
                        }
                    }
                }
                if (response.IsOnline == false)
                {
                    await DisplayAlert(AppResources.Warning, AppResources.NoInternetConnection, AppResources.OK);
                }
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //show my community
            foreach (var item in this.container.Children)
            {
                if (item is MyCommunityPage == true)
                {
                    item.IsVisible = true;
                }
                else
                {
                    item.IsVisible = false;
                }
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            foreach (var item in this.container.Children)
            {
                if (item is WhoAlertsMePage == true)
                {
                    item.IsVisible = true;
                }
                else
                {
                    item.IsVisible = false;
                }
            }
        }
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            foreach (var item in this.container.Children)
            {
                if (item is DependandsPage == true)
                {
                    item.IsVisible = true;
                }
                else
                {
                    item.IsVisible = false;
                }
            }
        }
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            foreach (var item in this.container.Children)
            {
                if (item is BlockedUsersPage == true)
                {
                    item.IsVisible = true;
                }
                else
                {
                    item.IsVisible = false;
                }
            }
        }

        private void contactList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tabsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}