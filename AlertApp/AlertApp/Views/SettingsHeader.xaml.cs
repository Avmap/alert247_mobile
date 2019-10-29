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
	public partial class SettingsHeader : ContentView
	{
        #region Title (Bindable string)
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                                  "Title", //Public name to use
                                                                  typeof(string), //this type
                                                                  typeof(SettingsHeader), //parent type (tihs control)
                                                                  string.Empty); //default value
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion Title (Bindable string)


        public SettingsHeader ()
		{
			InitializeComponent ();
        }
	}
}