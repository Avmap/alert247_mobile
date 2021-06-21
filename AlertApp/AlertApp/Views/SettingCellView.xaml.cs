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
	public partial class SettingCellView : ContentView
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

        #region Icon (Bindable string)
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
                                                                  "Icon", //Public name to use
                                                                  typeof(string), //this type
                                                                  typeof(SettingsHeader), //parent type (tihs control)
                                                                  string.Empty); //default value

        
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion Icon (Bindable string)
        
        public SettingCellView ()
		{
			InitializeComponent ();
		}

	}
}