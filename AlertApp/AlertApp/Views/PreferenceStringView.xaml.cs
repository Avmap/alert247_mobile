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
    public partial class PreferenceStringView : ContentView
    {
        #region Label (Bindable string)
        public static readonly BindableProperty LabelProperty = BindableProperty.Create(
                                                                  "Label", //Public name to use
                                                                  typeof(double?), //this type
                                                                  typeof(PreferenceStringView), //parent type (tihs control)
                                                                  null); //default value
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion Label (Bindable string)

        #region Text (Bindable string)
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                                                                  "Text", //Public name to use
                                                                  typeof(double?), //this type
                                                                  typeof(PreferenceStringView), //parent type (tihs control)
                                                                  string.Empty); //default value
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion Text (Bindable string)

        public PreferenceStringView()
        {
            InitializeComponent();
        }
    }
}