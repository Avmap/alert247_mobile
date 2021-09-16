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
            //vm = ViewModelLocator.Instance.Resolve<RegistrationFieldsPageViewModel>();
            //this.BindingContext = vm;
            var ups = ViewModelLocator.Instance.Resolve<Services.Profile.IUserProfileService>();
            var lss = ViewModelLocator.Instance.Resolve<Services.Settings.ILocalSettingsService>();
            var rss = ViewModelLocator.Instance.Resolve<Services.Registration.IRegistrationService>();
            var css = ViewModelLocator.Instance.Resolve<Services.Cryptography.ICryptographyService>();
            vm = new RegistrationFieldsPageViewModel(ups, lss, rss, css);  
            AddRegistrationFields(registrationField);


            var profileTapRecognizer = new TapGestureRecognizer
            {
                TappedCallback = (v, o) =>
                {
                    Task.Run(async () =>
                    {
                        SubmitRegistrationButton_Clicked(null,null);
                    });

                },
                NumberOfTapsRequired = 1
            };

            //nextImage.GestureRecognizers.Add(profileTapRecognizer); //todo PAUL
            topNextImage.GestureRecognizers.Add(profileTapRecognizer);

        }

        private void AddRegistrationFields(RegistrationField[] registrationField)
        {
            var language = _localSettingsService.GetSelectedLanguage();
            foreach (var item in registrationField)
            {
                string label = "";
                if (item.Labels != null)
                {
                    item.Labels.TryGetValue(language, out label);
                }
                switch (item.DataType)
                {
                    case RegistrationField.Type.String:
                        var entry = new NoUnderlineEntry { FieldName = item.FieldName, Placeholder = label, VerticalOptions = LayoutOptions.Center, FontSize = 16, HorizontalOptions = LayoutOptions.FillAndExpand, ReturnType = ReturnType.Next, Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] };
                        entry.TextChanged += Entry_TextChanged;
                        registrationContainer.Children.Add(entry);
                        break;
                        //case RegistrationField.Type.Date:
                        //    StackLayout horizontalstack = new StackLayout();
                        //    horizontalstack.Orientation = StackOrientation.Horizontal;
                        //    horizontalstack.HorizontalOptions = LayoutOptions.FillAndExpand;
                        //    horizontalstack.Children.Add(new Image { Source = "calendar.png", WidthRequest = 30, HeightRequest = 30 });
                        //    var datePicker = new DatePickerNullable { Style = (Style)Application.Current.Resources["RegistrationEntryStyle"], HorizontalOptions = LayoutOptions.FillAndExpand };
                        //    if (Device.RuntimePlatform == Device.iOS)
                        //        datePicker.BackgroundColor = Color.FromHex("#E6E7E8");
                        //    datePicker.DateSelected += DatePicker_DateSelected;                        
                        //    datePicker.OnDialogUnFocused += DatePicker_OnDialogUnFocused;
                        //    horizontalstack.Children.Add(datePicker);
                        //    var tapGestureRecognizer = new TapGestureRecognizer();
                        //    tapGestureRecognizer.Tapped += (s, e) => OnLabelClicked(s, e);
                        //    var clearTextLabel = new Xamarin.Forms.Label {IsVisible = false, Text = "X", WidthRequest = 20, HeightRequest = 20, VerticalOptions = LayoutOptions.Center };
                        //    clearTextLabel.GestureRecognizers.Add(tapGestureRecognizer);
                        //    horizontalstack.Children.Add(clearTextLabel);
                        //    var cardViewDate = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        //    cardViewDate.Content = horizontalstack;

                        //    stack.Children.Add(cardViewDate);
                        //    registrationContainer.Children.Add(stack);
                        //    break;
                        //case RegistrationField.Type.Boolean:
                        //    var switchControl = new Switch { };
                        //    switchControl.HorizontalOptions = LayoutOptions.Start;

                        //    var cardViewBoolean = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        //    cardViewBoolean.Content = switchControl;

                        //    stack.Children.Add(cardViewBoolean);
                        //    registrationContainer.Children.Add(stack);
                        //    break;
                        //case RegistrationField.Type.Area:

                        //    var cardViewEditor = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        //    var editor = new NoUnderlineEditor { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 150 };
                        //    if (Device.RuntimePlatform == Device.iOS)
                        //        editor.BackgroundColor = Color.FromHex("#E6E7E8");
                        //    editor.TextChanged += Entry_TextChanged;
                        //    cardViewEditor.Content = editor;
                        //    stack.Children.Add(cardViewEditor);
                        //    registrationContainer.Children.Add(stack);
                        //    break;
                }
            }
            vm.SetBusy(false);
        }

        private void AddRegistrationFields_old(RegistrationField[] registrationField)
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
                string label = "";
                if (item.Labels != null)
                {
                    
                    item.Labels.TryGetValue(language, out label);
                    //stack.Children.Add(new Xamarin.Forms.Label { Text = label, VerticalOptions = LayoutOptions.Center, WidthRequest = 100, Style = (Style)Application.Current.Resources["RegistrationLabelStyle"] });
                }
                stack.FieldName = item.FieldName;
                switch (item.DataType)
                {
                    case RegistrationField.Type.String:

                        var entry = new NoUnderlineEntry { Placeholder = label,  VerticalOptions = LayoutOptions.Center, FontSize = 16, HorizontalOptions = LayoutOptions.FillAndExpand, ReturnType = ReturnType.Next, Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] };
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
                        var datePicker = new DatePickerNullable { Style = (Style)Application.Current.Resources["RegistrationEntryStyle"], HorizontalOptions = LayoutOptions.FillAndExpand };
                        if (Device.RuntimePlatform == Device.iOS)
                            datePicker.BackgroundColor = Color.FromHex("#E6E7E8");
                        datePicker.DateSelected += DatePicker_DateSelected;
                        datePicker.OnDialogUnFocused += DatePicker_OnDialogUnFocused;
                        horizontalstack.Children.Add(datePicker);
                        var tapGestureRecognizer = new TapGestureRecognizer();
                        tapGestureRecognizer.Tapped += (s, e) => OnLabelClicked(s, e);
                        var clearTextLabel = new Xamarin.Forms.Label { IsVisible = false, Text = "X", WidthRequest = 20, HeightRequest = 20, VerticalOptions = LayoutOptions.Center };
                        clearTextLabel.GestureRecognizers.Add(tapGestureRecognizer);
                        horizontalstack.Children.Add(clearTextLabel);
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

                        var cardViewEditor = new Frame { HasShadow = false, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#E6E7E8"), CornerRadius = 4, BorderColor = Color.FromHex("#CACCCD"), Padding = new Thickness(2, 0, 2, 0) };
                        var editor = new NoUnderlineEditor { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 150 };
                        if (Device.RuntimePlatform == Device.iOS)
                            editor.BackgroundColor = Color.FromHex("#E6E7E8");
                        editor.TextChanged += Entry_TextChanged;
                        cardViewEditor.Content = editor;
                        stack.Children.Add(cardViewEditor);
                        registrationContainer.Children.Add(stack);
                        break;
                }
            }
            vm.SetBusy(false);
        }

        private void DatePicker_OnDialogUnFocused(object sender, EventArgs e)
        {            
            Entry_TextChanged(null, null);
        }

        private void OnLabelClicked(object s, EventArgs e)
        {

            var label = s as Xamarin.Forms.Label;
            if (label != null)
            {
                var parentView = label.Parent as StackLayout;
                if (parentView != null)
                {
                    foreach (var item in parentView.Children)
                    {
                        if (item is DatePickerNullable)
                        {
                            ((DatePickerNullable)item).CleanDate();
                            ((DatePickerNullable)item).NullableDate = null;
                            label.IsVisible = false;
                        }
                    }
                }
            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Entry_TextChanged(null, null);
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
            var hasValue = false;
            var registrationValues = new Dictionary<string, string>();
            foreach (var registrationView in registrationContainer.Children)
            {
                var registrationStackLayout = registrationView as RegistrationStackLayout;
                if (registrationStackLayout != null)
                {
                    var frame = registrationStackLayout.Children[registrationStackLayout.Children.Count-1] as Frame;
                    if (frame.Children.Count > 0 && frame.Children[0] is StackLayout)
                    {
                        var stackLayout = frame.Children[0] as StackLayout;
                        if (stackLayout.Children.Count > 2 && stackLayout.Children[1] is DatePickerNullable)
                        {
                            if (((DatePickerNullable)stackLayout.Children[1]).NullableDate.HasValue)
                            {
                                stackLayout.Children[2].IsVisible = true;
                                hasValue = true;                                
                            }
                                
                        }
                    }
                    foreach (var field in frame.Children)
                    {
                        if (field is Entry)
                        {
                            if (!string.IsNullOrWhiteSpace(((Entry)field).Text))
                                hasValue = true;
                        }
                        else if (field is DatePickerNullable)
                        {
                            hasValue = (((DatePickerNullable)field).NullableDate.HasValue);                            
                        }
                        else if (field is Editor)
                        {
                            if (!string.IsNullOrWhiteSpace(((Editor)field).Text))
                                hasValue = true;
                        }
                        else if (field is Switch)
                        {
                            registrationValues.Add(registrationStackLayout.FieldName, ((Switch)field).IsToggled.ToString().ToLower());
                        }
                    }
                }
            }
            return hasValue;
        }
        private async void SubmitRegistrationButton_Clicked(object sender, EventArgs e)
        {
            var registrationValues = new Dictionary<string, string>();
            foreach (var registrationView in registrationContainer.Children)
            {
                var registrationStackLayout = registrationView as NoUnderlineEntry;//RegistrationStackLayout;
                if (registrationStackLayout != null)
                {
                    registrationValues.Add(((NoUnderlineEntry)registrationStackLayout).FieldName, registrationStackLayout.Text);
                    //foreach (var field in registrationStackLayout.Children)
                    //{
                    //    if (field is Frame)
                    //        CollectValues(registrationValues, registrationStackLayout.FieldName, field as Frame);
                    //}
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
                else if (field is DatePickerNullable)
                {
                    if (((DatePickerNullable)field).NullableDate.HasValue)
                        registrationValues.Add(fieldName, ((DatePickerNullable)field).NullableDate.Value.Date.ToString("dd/MM/yyyy HH:mm"));
                }
                else if (field is Editor)
                {
                    registrationValues.Add(fieldName, ((Editor)field).Text);
                }
                else if (field is Switch)
                {
                    registrationValues.Add(fieldName, ((Switch)field).IsToggled.ToString().ToLower());
                }
                else if (field is StackLayout)
                {
                    var stack = field as StackLayout;
                    if (stack.Children[1] is DatePickerNullable)
                    {
                        if (((DatePickerNullable)stack.Children[1]).NullableDate.HasValue)
                            registrationValues.Add(fieldName, ((DatePickerNullable)stack.Children[1]).NullableDate.Value.Date.ToString("dd/MM/yyyy HH:mm"));
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