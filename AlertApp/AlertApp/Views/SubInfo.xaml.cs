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
	public partial class SubInfo : ContentView
	{

        public static BindableProperty SubscriptionStartProperty = BindableProperty.Create(
                                                          "SubscriptionStart", //Public name to use
                                                          typeof(string), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          ""); //default value

        public string SubscriptionStart
        {
            get { return (string)GetValue(SubscriptionStartProperty); }
            set { SetValue(SubscriptionStartProperty, value); }
        }

        public static BindableProperty SubscriptionEndProperty = BindableProperty.Create(
                                                          "SubscriptionEnd", //Public name to use
                                                          typeof(string), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          ""); //default value

        public string SubscriptionEnd
        {
            get { return (string)GetValue(SubscriptionEndProperty); }
            set { SetValue(SubscriptionEndProperty, value); }
        }

        public static BindableProperty SubscriptionPackageProperty = BindableProperty.Create(
                                                          "SubscriptionPackage", //Public name to use
                                                          typeof(string), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          ""); //default value

        public string SubscriptionPackage
        {
            get { return (string)GetValue(SubscriptionPackageProperty); }
            set { SetValue(SubscriptionPackageProperty, value); }
        }

        public static BindableProperty IsSubOKProperty = BindableProperty.Create(
                                                          "IsSubOK", //Public name to use
                                                          typeof(bool), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          true); //default value

        public bool IsSubOK
        {
            get { return (bool)GetValue(IsSubOKProperty); }
            set { SetValue(IsSubOKProperty, value); }
        }

        public static BindableProperty IsSubExpiringProperty = BindableProperty.Create(
                                                          "IsSubExpiring", //Public name to use
                                                          typeof(bool), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          false); //default value

        public bool IsSubExpiring
        {
            get { return (bool)GetValue(IsSubExpiringProperty); }
            set { SetValue(IsSubExpiringProperty, value); }
        }

        public static BindableProperty IsSubExpiredProperty = BindableProperty.Create(
                                                          "IsSubExpired", //Public name to use
                                                          typeof(bool), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          false); //default value

        public bool IsSubExpired
        {
            get { return (bool)GetValue(IsSubExpiredProperty); }
            set { SetValue(IsSubExpiredProperty, value); }
        }

        public static BindableProperty IsSubInactiveProperty = BindableProperty.Create(
                                                          "IsSubInactive", //Public name to use
                                                          typeof(bool), //this type
                                                          typeof(SubInfo), //parent type (tihs control)
                                                          false); //default value

        public bool IsSubInactive
        {
            get { return (bool)GetValue(IsSubInactiveProperty); }
            set { SetValue(IsSubInactiveProperty, value); }
        }

        public SubInfo ()
		{
			InitializeComponent ();
		}
	}
}