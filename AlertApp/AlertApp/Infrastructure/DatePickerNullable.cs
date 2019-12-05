using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class DatePickerNullable : DatePicker
    {
        public delegate void DialogUnFocusedHandler(object sender, EventArgs e);
        public event DialogUnFocusedHandler OnDialogUnFocused;

        public string _format = null;
        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create<DatePickerNullable, DateTime?>(p => p.NullableDate, null);
        public DatePickerNullable()
        {
           // this.Unfocused += DatePickerNullable_Unfocused;

        }

        private void DatePickerNullable_Unfocused(object sender, FocusEventArgs e)
        {
            var datePicker = sender as DatePickerNullable;            
            this.NullableDate = datePicker.Date;
            // check if a handler is assigned
            if (OnDialogUnFocused != null)
            {
                OnDialogUnFocused(this, null);
            }
        }

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        private void UpdateDate()
        {
            if (NullableDate.HasValue)
            {
                if (null != _format)
                {
                    _format = "dd/MM/yyyy";
                    Format = _format;
                    Date = NullableDate.Value;
                }

            }
            else
            {
                _format = Format;
                Format = "  ";
            }
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateDate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "Date")
            {
                NullableDate = Date;
            }

            if (propertyName == NullableDateProperty.PropertyName && NullableDate.HasValue)
            {
                Date = NullableDate.Value;
                if (Date.ToString(_format) == DateTime.Now.ToString(_format))
                {
                    //this code was done because when date selected is the actual date the"DateProperty" does not raise  
                    UpdateDate();
                }
            }

        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }

        public void AssignValue()
        {
            NullableDate = Date;
            UpdateDate();
            OnDialogUnFocused(this, null);
        }
    }

}