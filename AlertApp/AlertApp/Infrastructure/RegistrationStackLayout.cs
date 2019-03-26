using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class RegistrationStackLayout : StackLayout
    {
        public static readonly BindableProperty FieldNameProperty = BindableProperty.Create("FieldName", typeof(string), typeof(RegistrationStackLayout), null, BindingMode.TwoWay, null);

        public string FieldName
        {
            get
            {
                return (string)GetValue(FieldNameProperty);
            }
            set
            {
                SetValue(FieldNameProperty, value);
            }
        }
    }
}
