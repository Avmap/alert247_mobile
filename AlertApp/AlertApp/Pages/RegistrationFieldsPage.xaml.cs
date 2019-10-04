using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationFieldsPage : ContentPage
    {
        readonly ILocalSettingsService _localSettingsService;
        RegistrationFieldsPageViewModel vm;
        public RegistrationFieldsPage(RegistrationField[] registrationField)
        {
            InitializeComponent();
            _localSettingsService = new LocalSettingsService();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            vm = ViewModelLocator.Instance.Resolve<RegistrationFieldsPageViewModel>();
            this.BindingContext = vm;
            AddRegistrationFields(registrationField);


            var profileTapRecognizer = new TapGestureRecognizer
            {
                TappedCallback = (v, o) =>
                {
                    Task.Run(async () =>
                    {
                        SubmitRegistrationButton_Clicked();
                    });

                },
                NumberOfTapsRequired = 1
            };

            nextImage.GestureRecognizers.Add(profileTapRecognizer);

        }


        private void AddRegistrationFields(RegistrationField[] registrationField)
        {
#if DEBUG
            //var list = new List<RegistrationField>();
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.Date, FieldName = "birthdate", Labels = new Dictionary<string, string>() });
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "area", Labels = new Dictionary<string, string>() });
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "firstname", Labels = new Dictionary<string, string>() });
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "firstname2", Labels = new Dictionary<string, string>() });
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.Boolean, FieldName = "epilispi", Labels = new Dictionary<string, string>() });
            //list.Add(new RegistrationField { DataType = RegistrationField.Type.Area, FieldName = "other", Labels = new Dictionary<string, string>() });
            //registrationField = list.ToArray();
#endif            
            var language = _localSettingsService.GetSelectedLanguage();
            foreach (var item in registrationField)
            {
                RegistrationStackLayout stack = new RegistrationStackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                if (item.Labels != null)
                {
                    string label = "";
                    item.Labels.TryGetValue(language, out label);
                    //#if DEBUG
                    //                    stack.Children.Add(new Xamarin.Forms.Label { VerticalOptions = LayoutOptions.Center, WidthRequest = 100, Text = "Registration field", Style = (Style)Application.Current.Resources["RegistrationLabelStyle"] });
                    //#else
                    stack.Children.Add(new Xamarin.Forms.Label { Text = label, VerticalOptions = LayoutOptions.Center, WidthRequest = 100, Style = (Style)Application.Current.Resources["RegistrationLabelStyle"] });
                    //#endif

                }
                stack.FieldName = item.FieldName;
                switch (item.DataType)
                {
                    case RegistrationField.Type.String:
                        stack.Children.Add(new Entry { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, ReturnType = ReturnType.Next, Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] });
                        registrationContainer.Children.Add(stack);
                        break;
                    case RegistrationField.Type.Date:
                        StackLayout horizontalstack = new StackLayout();
                        horizontalstack.Orientation = StackOrientation.Horizontal;
                        horizontalstack.HorizontalOptions = LayoutOptions.FillAndExpand;
                        horizontalstack.Children.Add(new Image { Source = "calendar.png", WidthRequest = 30, HeightRequest = 30 });
                        horizontalstack.Children.Add(new DatePicker { Style = (Style)Application.Current.Resources["RegistrationEntryStyle"], HorizontalOptions = LayoutOptions.FillAndExpand });
                        stack.Children.Add(horizontalstack);
                        registrationContainer.Children.Add(stack);
                        break;
                    case RegistrationField.Type.Boolean:
                        var switchControl = new Switch { };
                        switchControl.HorizontalOptions = LayoutOptions.Start;
                        stack.Children.Add(switchControl);
                        registrationContainer.Children.Add(stack);
                        break;
                    case RegistrationField.Type.Area:
                        stack.Children.Add(new Editor { VerticalOptions = LayoutOptions.Center, Text = "asdasdasdasds", AutoSize = EditorAutoSizeOption.TextChanges });
                        registrationContainer.Children.Add(stack);
                        break;
                }
            }
            vm.SetBusy(false);
        }

        private async void SubmitRegistrationButton_Clicked()
        {
            var registrationValues = new Dictionary<string, string>();
            foreach (var registrationView in registrationContainer.Children)
            {
                var registrationStackLayout = registrationView as RegistrationStackLayout;
                if (registrationStackLayout != null)
                {
                    foreach (var field in registrationStackLayout.Children)
                    {
                        if (field is Entry)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((Entry)field).Text);
                        }
                        else if (field is DatePicker)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((DatePicker)field).Date.Date.ToString());
                        }
                        else if (field is Editor)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((Editor)field).Text);
                        }
                        else if (field is Switch)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((Switch)field).IsToggled.ToString().ToLower());
                        }
                    }
                }
            }
            vm.SendUserProfile(registrationValues);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}