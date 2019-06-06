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

        }


        private void AddRegistrationFields(RegistrationField[] registrationField)
        {
#if DEBUG
            registrationField.ToList().Add(new RegistrationField { DataType = RegistrationField.Type.Date, FieldName = "date_birth", Labels = new Dictionary<string, string>() });
#endif            
            var language = _localSettingsService.GetSelectedLanguage();
            foreach (var item in registrationField)
            {
                RegistrationStackLayout stack = new RegistrationStackLayout();
                stack.Orientation = StackOrientation.Vertical;
                if (item.Labels != null)
                {
                    string label = "";
                    item.Labels.TryGetValue(language, out label);
                    stack.Children.Add(new Xamarin.Forms.Label { Text = label, Style = (Style)Application.Current.Resources["RegistrationLabelStyle"] });
                }                
                stack.FieldName = item.FieldName;                
                switch (item.DataType)
                {
                    case RegistrationField.Type.String:
                        stack.Children.Add(new Entry { Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] });
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
                }
            }
            vm.SetBusy(false);
        }

        private async void SubmitRegistrationButton_Clicked(object sender, EventArgs e)
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
                            registrationValues.Add(registrationStackLayout.FieldName,((Entry)field).Text);
                        }
                        else if (field is DatePicker)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((DatePicker)field).Date.Date.ToString());
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