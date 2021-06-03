using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                //    ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            }
            //Application.Current.MainPage.Navigation
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            //this.BindingContext = ViewModelLocator.Instance.Resolve<MainPageViewModel>();
            var ups = ViewModelLocator.Instance.Resolve<Services.Profile.IUserProfileService>();
            var lss = ViewModelLocator.Instance.Resolve<Services.Settings.ILocalSettingsService>();
            var cs = ViewModelLocator.Instance.Resolve<Services.Contacts.IContactsService>();
            var ns = ViewModelLocator.Instance.Resolve<Services.News.INewsService>();
            var ss = ViewModelLocator.Instance.Resolve<Services.Subscription.ISubscriptionService>();

            this.BindingContext = new MainPageViewModel(ups, lss, cs, ns,ss);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MainPageViewModel)this.BindingContext).HideShowButtons();
            RegisterForRefreshContacts();
            RefreshContacts();
            ((MainPageViewModel)this.BindingContext).RefreshNews();
            ((MainPageViewModel)this.BindingContext).RefreshSubInfo();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnRegisterForRefreshContacts();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}

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
                var vm = this.BindingContext as MainPageViewModel;
                int cnt = await vm.GetContacts();
                vm.NumberOfContacts = cnt;
            });
        }

    }
}