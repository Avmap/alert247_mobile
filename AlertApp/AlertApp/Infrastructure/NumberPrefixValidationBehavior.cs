using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class NumberPrefixValidationBehavior : Behavior<Entry>
    {
        string countryCodeRegex = @"^\+\d+$";


        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, countryCodeRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            var isOnlyPlus = e.NewTextValue.Length == 1 && e.NewTextValue == "+";
           
            if (!IsValid && !isOnlyPlus)
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
            // ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}
