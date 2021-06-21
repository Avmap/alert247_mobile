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
using static AlertApp.ViewModels.ManageContactsPageViewModel;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageContactsPage : ContentPage
    {
        int tabsAdded = 0;
        int tabsCount = 3;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        readonly IContacts _contacts;
        public int MyProperty { get; set; }
        public ManageContactsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
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
            //this.container.Children.Add(dependantsPage);  //todo: enable when ready
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
                TabItem whoAlertmeTab = vm.Tabs.ToList().Where(x => x.Name == AppResources.WhoAlertMe).First();
                if (response != null)
                {
                    foreach (var item in this.container.Children)
                    {
                        if (item.BindingContext is IHaveContacts)
                        {
                            ((IHaveContacts)item.BindingContext).SetContacts(response, addressBook);
                            if (item.BindingContext is WhoAlertsMePageViewModel)
                            {
                                if (((WhoAlertsMePageViewModel)item.BindingContext).NotificationCount > 0)
                                {

                                    whoAlertmeTab.HasBadge = true;
                                    whoAlertmeTab.NotificationCount = ((WhoAlertsMePageViewModel)item.BindingContext).NotificationCount;
                                }
                                else
                                {
                                    whoAlertmeTab.HasBadge = false;
                                }
                            }
                        }
                    }
                }
                if (response.IsOnline == false)
                {
                    await DisplayAlert(AppResources.Warning, AppResources.NoInternetConnection, AppResources.OK);
                }
            });
        }

        private void tabsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = this.BindingContext as ManageContactsPageViewModel;
            if (e.PreviousSelection != null && e.PreviousSelection.Count > 0)
            {
                var previusTabItem = e.PreviousSelection.FirstOrDefault() as TabItem;
                previusTabItem.Selected = false;
            }
            else
            {
                //only from first time click. At first time PreviousSelection has Count 0               
                vm.Tabs[0].Selected = false;
            }


            var currentTabItem = e.CurrentSelection.FirstOrDefault() as TabItem;
            currentTabItem.Selected = true;
            //TabItem whoAlertmeTab = vm.Tabs.ToList().Where(x => x.Name == AppResources.WhoAlertMe).First();
            for (var i = 0; i < this.container.Children.Count; i++)
            {
                this.container.Children[i].IsVisible = false;
            }
            
            //switch (currentTabItem.Name){
            //    case AppResources.TabMyCommunity:

            //        break;
            //    case AppResources.TabDependands:

            //        break;
            //    case AppResources.WhoAlertMe:

            //        break;
            //    case AppResources.BlockedUsersPage:

            //        break;

            //}
            //this.container.Children[currentTabItem.Id - 1].IsVisible = true;
            switch (currentTabItem.Id)
            {
                case 1:
                    this.container.Children.Where(x => x.GetType() == typeof(MyCommunityPage)).First().IsVisible = true;
                    //this.container.Children[0].IsVisible = true;
                    //this.container.Children[1].IsVisible = false;
                    //this.container.Children[2].IsVisible = false;
                    //this.container.Children[3].IsVisible = false;
                    addCommunityContact.IsVisible = true;
                    //contactsMenu.IsVisible = false;
                    tabsList.ScrollTo(0, position: ScrollToPosition.Start);
                    break;
                case 2:
                    this.container.Children.Where(x => x.GetType() == typeof(DependandsPage)).First().IsVisible = true;
                    //this.container.Children[0].IsVisible = false;
                    //this.container.Children[1].IsVisible = true;
                    //this.container.Children[2].IsVisible = false;
                    //this.container.Children[3].IsVisible = false;
                    addCommunityContact.IsVisible = false;
                    //contactsMenu.IsVisible = true;
                    break;
                case 3:
                    this.container.Children.Where(x => x.GetType() == typeof(WhoAlertsMePage)).First().IsVisible = true;
                    //this.container.Children[0].IsVisible = false;
                    //this.container.Children[1].IsVisible = false;
                    //this.container.Children[2].IsVisible = true;
                    //this.container.Children[3].IsVisible = false;
                    addCommunityContact.IsVisible = false;
                    //contactsMenu.IsVisible = true;
                    break;
                case 4:
                    this.container.Children.Where(x => x.GetType() == typeof(BlockedUsersPage)).First().IsVisible = true;
                    //this.container.Children[0].IsVisible = false;
                    //this.container.Children[1].IsVisible = false;
                    //this.container.Children[2].IsVisible = false;
                    //this.container.Children[3].IsVisible = true;
                    addCommunityContact.IsVisible = false;
                    //contactsMenu.IsVisible = true;
                    tabsList.ScrollTo(3, position: ScrollToPosition.Start);
                    break;
            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var myCommunityPageViewModel = this.container.Children[0].BindingContext as MyCommunityPageViewModel;

            var communityBlocked = myCommunityPageViewModel.Community.Select(c => c.Cellphone).ToList();
            if (myCommunityPageViewModel.Blocked != null && myCommunityPageViewModel.Blocked.Count > 0)
            {
                communityBlocked.AddRange(myCommunityPageViewModel.Blocked.Select(c => c.Cellphone));
            }

            var contactsPage = new AddContactPage(communityBlocked);

            contactsPage.Disappearing += (sender2, e2) =>
            {
                myCommunityPageViewModel.SetBusy(false);
                var vm = contactsPage.BindingContext as AddContactPageViewModel;
                if (vm.HasChange)
                {
                    foreach (var page in Application.Current.MainPage.Navigation.NavigationStack)
                    {
                        if (page is ManageContactsPage)
                        {
                            myCommunityPageViewModel.SetBusy(true);
                            ((ManageContactsPage)page).RefreshContacts();
                            break;
                        }
                    }
                }
            };
            await Application.Current.MainPage.Navigation.PushAsync(contactsPage, false);
        }
    }
}