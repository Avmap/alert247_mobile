using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AlertApp.Model.Api;
using System.Windows.Input;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsFeed : ContentView
    {
        public NewsFeed()
        {
            InitializeComponent();
        }

        public static BindableProperty NewsSourceProperty = BindableProperty.Create(
                                                          "NewsSource", //Public name to use
                                                          typeof(ObservableCollection<NewsEntry>), //this type
                                                          typeof(NewsFeed), //parent type (tihs control)
                                                          new ObservableCollection<NewsEntry>()); //default value

        public ObservableCollection<NewsEntry> NewsSource
        {
            get { return (ObservableCollection<NewsEntry>)GetValue(NewsSourceProperty); }
            set { SetValue(NewsSourceProperty, value); }
        }

        public static BindableProperty IsRefreshingProperty = BindableProperty.Create(
                                                         "IsRefreshing", //Public name to use
                                                         typeof(bool), //this type
                                                         typeof(NewsFeed), //parent type (tihs control)
                                                         false, //default value
                                                         BindingMode.TwoWay);

        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        public static BindableProperty RefreshCommandProperty = BindableProperty.Create(
                                                          "RefreshCommand", //Public name to use
                                                          typeof(ICommand), //this type
                                                          typeof(NewsFeed), //parent type (tihs control)
                                                          (ICommand)MyRefreshNewsCommand); //default value

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        private static ICommand MyRefreshNewsCommand
        {
            get
            {
                return new Command(DoNothing);
            }
        }

        private static void DoNothing() { }
    }
}