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
            topNextImage.GestureRecognizers.Add(profileTapRecognizer);

        }


        private void AddRegistrationFields(RegistrationField[] registrationField)
        {
//#if DEBUG
//            var list = new List<RegistrationField>();
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.Date, FieldName = "birthdate", Labels = new Dictionary<string, string>() });
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "area", Labels = new Dictionary<string, string>() });
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "firstname", Labels = new Dictionary<string, string>() });
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.String, FieldName = "firstname2", Labels = new Dictionary<string, string>() });
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.Boolean, FieldName = "epilispi", Labels = new Dictionary<string, string>() });
//            list.Add(new RegistrationField { DataType = RegistrationField.Type.Area, FieldName = "other", Labels = new Dictionary<string, string>() });
//            registrationField = list.ToArray();
//#endif            
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

                        var entry = new NoUnderlineEntry { VerticalOptions = LayoutOptions.Center, FontSize = 16, HorizontalOptions = LayoutOptions.FillAndExpand, ReturnType = ReturnType.Next, Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] };
                        entry.TextChanged += Entry_TextChanged;
                        var cardView = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        cardView.Content = entry;
                        stack.Children.Add(cardView);
                        registrationContainer.Children.Add(stack);

                        break;
                    case RegistrationField.Type.Date:
                        StackLayout horizontalstack = new StackLayout();
                        horizontalstack.Orientation = StackOrientation.Horizontal;
                        horizontalstack.HorizontalOptions = LayoutOptions.FillAndExpand;
                        horizontalstack.Children.Add(new Image { Source = "calendar.png", WidthRequest = 30, HeightRequest = 30 });
                        horizontalstack.Children.Add(new DatePicker { Style = (Style)Application.Current.Resources["RegistrationEntryStyle"], HorizontalOptions = LayoutOptions.FillAndExpand });

                        var cardViewDate = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        cardViewDate.Content = horizontalstack;

                        stack.Children.Add(cardViewDate);
                        registrationContainer.Children.Add(stack);
                        break;
                    case RegistrationField.Type.Boolean:
                        var switchControl = new Switch { };
                        switchControl.HorizontalOptions = LayoutOptions.Start;

                        var cardViewBoolean = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        cardViewBoolean.Content = switchControl;

                        stack.Children.Add(cardViewBoolean);
                        registrationContainer.Children.Add(stack);
                        break;
                    case RegistrationField.Type.Area:

                        var cardViewEditor = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        cardViewEditor.Content = new Editor { VerticalOptions = LayoutOptions.Center, AutoSize = EditorAutoSizeOption.TextChanges };
                        stack.Children.Add(cardViewEditor);
                        registrationContainer.Children.Add(stack);
                        break;
                }
            }
            vm.SetBusy(false);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HasValue())
            {
                labelPrompt.IsVisible = false;
                topNextButton.IsVisible = true;
                bottomNextButton.IsVisible = false;
            }
            else
            {
                labelPrompt.IsVisible = true;
                topNextButton.IsVisible = false;
                bottomNextButton.IsVisible = true;
            }
        }

        private bool HasValue()
        {
            var registrationValues = new Dictionary<string, string>();
            foreach (var registrationView in registrationContainer.Children)
            {
                var registrationStackLayout = registrationView as RegistrationStackLayout;
                if (registrationStackLayout != null)
                {
                    var frame = registrationStackLayout.Children[1] as Frame;
                    foreach (var field in frame.Children)
                    {
                        if (field is Entry)
                        {
                            if (!string.IsNullOrWhiteSpace(((Entry)field).Text))
                                return true;
                        }
                        else if (field is DatePicker)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((DatePicker)field).Date.Date.ToString());
                        }
                        else if (field is Editor)
                        {
                            if (!string.IsNullOrWhiteSpace(((Editor)field).Text))
                                return true;
                        }
                        else if (field is Switch)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((Switch)field).IsToggled.ToString().ToLower());
                        }
                    }
                }
            }
            return false;
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
                        if (field is Frame)
                            CollectValues(registrationValues, registrationStackLayout.FieldName, field as Frame);
                    }
                }
            }
            vm.SendUserProfile(registrationValues);
        }

        private void CollectValues(Dictionary<string, string> registrationValues, string fieldName, Frame parent)
        {

            foreach (var field in parent.Children)
            {
                if (field is Entry)
                {
                    registrationValues.Add(fieldName, ((Entry)field).Text);
                }
                else if (field is DatePicker)
                {
                    registrationValues.Add(fieldName, ((DatePicker)field).Date.Date.ToString());
                }
                else if (field is Editor)
                {
                    registrationValues.Add(fieldName, ((Editor)field).Text);
                }
                else if (field is Switch)
                {
                    registrationValues.Add(fieldName, ((Switch)field).IsToggled.ToString().ToLower());
                }
                else if (field is StackLayout )
                {
                    var stack = field as StackLayout;
                    if (stack.Children[1] is DatePicker)
                    {
                        registrationValues.Add(fieldName, ((DatePicker)stack.Children[1]).Date.Date.ToString());
                    }                   
                }
            }

        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}