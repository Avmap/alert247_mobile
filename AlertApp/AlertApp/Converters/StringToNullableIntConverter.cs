using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Converters
{
    public class StringToNullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string nonDecimalSeparator = ".";
            //var textValue = value as string;
            //if (culture.NumberFormat.CurrencyDecimalSeparator == ",")
            //{
            //    value = textValue.Replace(".", culture.NumberFormat.CurrencyDecimalSeparator);
            //}
            //else
            //{
            //    value = textValue.Replace(",", culture.NumberFormat.CurrencyDecimalSeparator);
            //}
            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;

            if (string.IsNullOrEmpty(strValue))
                return null;

            double resultInt;

            if (double.TryParse(strValue, out resultInt))
            {
                return resultInt;
            }
            return 0;
        }
    }
}