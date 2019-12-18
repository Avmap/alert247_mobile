using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class DecimalEntry : Entry
    {
        #region SupportNegative (Bindable boolean)
        public static readonly BindableProperty SupportNegativeProperty = BindableProperty.Create(
                                                                  "SupportNegativeValues", //Public name to use
                                                                  typeof(bool?), //this type
                                                                  typeof(DecimalEntry), //parent type (tihs control)
                                                                  false); //default value
        public bool SupportNegativeValues
        {
            get { return (bool)GetValue(SupportNegativeProperty); }
            set { SetValue(SupportNegativeProperty, value); }
        }
        #endregion SupportNegative (Bindable boolean)

        public DecimalEntry()
        {
            TextChanged += DecimalEntry_TextChanged;

            if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#E6E7E8");
                TextColor = Color.Black;
            }
        }

        private void DecimalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entryControl = sender as Entry;
            var culture = Resx.AppResources.Culture;
            string nonDecimalSeparator = ".";
            if (culture.NumberFormat.CurrencyDecimalSeparator == ",")
            {
                nonDecimalSeparator = ".";
            }
            double resultDouble;

            var newText = entryControl.Text.Replace(nonDecimalSeparator, culture.NumberFormat.CurrencyDecimalSeparator);
            var commasLength = newText.Split(',').Length;
            
            if (commasLength < 3 && double.TryParse(newText, out resultDouble) || string.IsNullOrWhiteSpace(e.NewTextValue) || newText == "-" || newText.StartsWith(".-") || newText.StartsWith("-."))
            {
                Text = newText;
            }
            else
            {
                Text = e.OldTextValue;
            }
        }
    }
}
