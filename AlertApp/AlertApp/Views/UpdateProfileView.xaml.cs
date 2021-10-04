using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Settings;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProfileView : ContentView
    {
        readonly ILocalSettingsService _localSettingsService;
        RegistrationFieldsPageViewModel vm;
        public UpdateProfileView()
        {
            InitializeComponent();
            _localSettingsService = new LocalSettingsService();
            vm = ViewModelLocator.Instance.Resolve<RegistrationFieldsPageViewModel>();
            this.BindingContext = vm;
            AddRegistrationFields();


        }
        private async void AddRegistrationFields()
        {
            vm.SetBusy(true);
            var registrationFieldsResponse = await vm.GetRegistrationFieldsAsync();
            if (!registrationFieldsResponse.IsOk && !registrationFieldsResponse.IsOnline)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.NoInternetConnection, "OK");
                Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
            }

            if (registrationFieldsResponse.Result == null)
            {
                return;
            }

            var userProfile = await vm.GetUserProfileAsync();
            if (!userProfile.IsOk && !userProfile.IsOnline)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.NoInternetConnection, "OK");
                Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
            }
            Dictionary<string, string> decryptedUserProfile = new Dictionary<string, string>();
            if (userProfile != null && userProfile.Result != null && userProfile.Result.Profile != null)
            {
                decryptedUserProfile = await vm.DecryptProfileAsync(userProfile.Result.Profile);
            }



            var language = _localSettingsService.GetSelectedLanguage();

            if (registrationFieldsResponse != null && registrationFieldsResponse.Result != null)
            {
                foreach (var item in registrationFieldsResponse.Result)
                {
                    if (item.FieldName == "weight" || item.FieldName == "medicine" || item.FieldName == "allergies")
                    {
                        continue;
                    }

                    RegistrationStackLayout stack = new RegistrationStackLayout();
                    stack.Orientation = StackOrientation.Horizontal;
                    if (item.Labels != null)
                    {
                        string label = "";
                        item.Labels.TryGetValue(language, out label);
                        stack.Children.Add(new Xamarin.Forms.Label { Text = label, VerticalOptions = LayoutOptions.Start, WidthRequest = 100, Style = (Style)Application.Current.Resources["RegistrationLabelStyle"] });
                    }
                    stack.FieldName = item.FieldName;
                    switch (item.DataType)
                    {
                        case RegistrationField.Type.String:

                            var entry = new NoUnderlineEntry { VerticalOptions = LayoutOptions.Center, FontSize = 16, HorizontalOptions = LayoutOptions.FillAndExpand, ReturnType = ReturnType.Next, Style = (Style)Application.Current.Resources["RegistrationEntryStyle"] };
                            //entry.TextChanged += Entry_TextChanged;
                            if (decryptedUserProfile.ContainsKey(item.FieldName))
                            {
                                entry.Text = decryptedUserProfile[item.FieldName];
                            }
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
                            if (decryptedUserProfile.ContainsKey(item.FieldName))
                            {
                                datePicker._format = "dd/MM/yyyy";
                                string birth = decryptedUserProfile[item.FieldName];
                                var datetime = DateTime.Parse(birth, new CultureInfo("en-GB"));
                                datePicker.Date = datetime.Date;
                                datePicker.AssignValue();
                            }

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
                           // editor.TextChanged += Entry_TextChanged;

                            if (decryptedUserProfile.ContainsKey(item.FieldName))
                            {
                                editor.Text = decryptedUserProfile[item.FieldName];
                            }
                            cardViewEditor.Content = editor;
                            stack.Children.Add(cardViewEditor);
                            registrationContainer.Children.Add(stack);
                            break;
                    }
                }
            }

            vm.SetBusy(false);
        }
        private void DatePicker_OnDialogUnFocused(object sender, EventArgs e)
        {
            //Entry_TextChanged(null, null);
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
            //Entry_TextChanged(null, null);
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
                    var frame = registrationStackLayout.Children[1] as Frame;
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
        private async Task<bool> SubmitRegistration()
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
            return await vm.UpdateUserProfile(registrationValues);
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
        private void ConfirmSettingsClick(object sender, EventArgs e)
        {
            if (!vm.Busy)
            {
                var confirmView = new ConfirmChangeView();
                var page = new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.Confirmation, confirmView);
                page.Disappearing += (sender2, e2) =>
                {
                    if (confirmView.Confirmed)
                    {
                        SaveChanges();
                    }
                };

                Navigation.PushModalAsync(page);
            }

        }
        private async void SaveChanges()
        {
            var updated = await SubmitRegistration();
            if (updated)
                Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
        }
    }
}