using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class DecimalEntry : Entry
    {
        public DecimalEntry()
        {
            TextChanged += DecimalEntry_TextChanged;
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
            if (double.TryParse(newText, out resultDouble) || string.IsNullOrWhiteSpace(e.NewTextValue) || newText == "-" || newText.StartsWith(".-") || newText.StartsWith("-."))
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
