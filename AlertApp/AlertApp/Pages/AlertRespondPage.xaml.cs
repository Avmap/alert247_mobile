﻿using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertRespondPage : ContentPage
    {
        NotificationAction _notificationAction;
        public AlertRespondPage(NotificationAction notificationAction)
        {
            InitializeComponent();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("notificationAction", notificationAction);
            this.BindingContext = ViewModelLocator.Instance.Resolve<AlertRespondPageViewModel>(parameters);
            _notificationAction = notificationAction;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = this.BindingContext as AlertRespondPageViewModel;
            vm.SetProfileData();
            SetAlertPosition();

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void SetAlertPosition()
        {
            try
            {
                //await Task.Delay(2000);
                var data = _notificationAction.Data as AlertNotificationData;
                if (!string.IsNullOrWhiteSpace(data.Position) && data.Position != ",")
                {
                    string[] latlng = data.Position.Split(',');
                    if (latlng.Length == 2)
                    {
                        var lat = Double.Parse(latlng[0], CultureInfo.InvariantCulture);
                        var lng = Double.Parse(latlng[1], CultureInfo.InvariantCulture);
                        var position = new Position(lat, lng);
                        var pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = position,
                            Label = "SOS"
                        };
                        map.Pins.Add(pin);

                        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, lng), Distance.FromMeters(200)));
                    }
                }
                else
                {
                    map.IsVisible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}