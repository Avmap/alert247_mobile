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
    public partial class Toolbar : ContentView
    {
        #region Title (Bindable string)
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                                  "Title", //Public name to use
                                                                  typeof(string), //this type
                                                                  typeof(Toolbar), //parent type (tihs control)
                                                                  string.Empty); //default value
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion Title (Bindable string)

        #region SubTitle (Bindable string)
        public static readonly BindableProperty SubTitleProperty = BindableProperty.Create(
                                                                  "SubTitle", //Public name to use
                                                                  typeof(string), //this type
                                                                  typeof(Toolbar), //parent type (tihs control)
                                                                  string.Empty); //default value
        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }
        #endregion SubTitle (Bindable string)

        #region HasBackButton (Bindable boolean)
        public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create(
                                                                  "HasBackButton", //Public name to use
                                                                  typeof(bool), //this type
                                                                  typeof(Toolbar), //parent type (tihs control)
                                                                  false); //default value
        public bool HasBackButton
        {
            get { return (bool)GetValue(HasBackButtonProperty); }
            set { SetValue(HasBackButtonProperty, value); }
        }
        #endregion SubTitle (Bindable boolean)

        public System.Windows.Input.ICommand BackCommand
        {
            get; set;
        }

        public static readonly BindableProperty BackCommandProperty =
            BindableProperty.Create(
                propertyName: "BackCommand",
                returnType: typeof(System.Windows.Input.ICommand),
                declaringType: typeof(Toolbar),
                defaultValue: null,
                propertyChanged: BackCommandPropertyPropertyChanged);

        private static void BackCommandPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tapRecognizer = new TapGestureRecognizer
            {
                Command = (System.Windows.Input.ICommand)newValue,
                NumberOfTapsRequired = 1
            };
            var control = (Toolbar)bindable;

            var frame = control.Content as ToolBarFrame;
            var grid = frame.Content as Grid;

            var backImg = grid.Children[0] as Image;
            if (backImg != null)
            {
                backImg.GestureRecognizers.Add(tapRecognizer);
            }


        }

        public Toolbar()
        {
            InitializeComponent();
        }
    }
}