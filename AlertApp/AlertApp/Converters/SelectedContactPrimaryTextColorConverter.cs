using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Converters
{
    class SelectedContactPrimaryTextColorConverter : IValueConverter
    {
        #region IValueConverter implementation
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selected = value as bool?;
            if (selected.HasValue && selected.Value)
            {
                return Xamarin.Forms.Color.White;
            }
            return Xamarin.Forms.Color.FromHex("#800000");
          
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion

    }
}