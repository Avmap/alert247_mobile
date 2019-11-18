using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Converters
{
    public class SelectedTabTextColorConverter : IValueConverter
    {
        #region IValueConverter implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selected = value as bool?;
            if (selected.HasValue && selected.Value)
            {
                return Color.FromHex("#800000");
            }
            return Color.Black;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion

    }
}