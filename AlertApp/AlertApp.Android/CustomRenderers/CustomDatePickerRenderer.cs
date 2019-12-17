using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AlertApp.Droid;
using Java.Util;
using System.Globalization;
using AlertApp.Utils;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(AlertApp.Infrastructure.DatePickerNullable), typeof(AlertApp.Droid.CustomRenderers.NullableDatePickerRenderer))]
namespace AlertApp.Droid.CustomRenderers
{
    public class NullableDatePickerRenderer : ViewRenderer<AlertApp.Infrastructure.DatePickerNullable, EditText>
    {
        DatePickerDialog _dialog;
        string cancel = Infrastructure.GlobalTranslates.Cancel;
        string ok = Infrastructure.GlobalTranslates.OK;
        protected override void OnElementChanged(ElementChangedEventArgs<AlertApp.Infrastructure.DatePickerNullable> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new EditText(Forms.Context));
            if (Control == null || e.NewElement == null)
                return;

            this.Control.Click += OnPickerClick;
            this.Control.Text = Element.Date.ToString(Element.Format);
            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
                SetDate(Element.Date);
        }

        void OnPickerFocusChange(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(DateTime date)
        {
            this.Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(this.Element.Date.Year, this.Element.Date.Month - 1, this.Element.Date.Day);
            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            var preferenceLanguage = Xamarin.Essentials.Preferences.Get(Settings.SelectedLanguage, "en");


            var ci = new CultureInfo(preferenceLanguage);

            var locales = Locale.GetAvailableLocales();
            Locale.Default = new Locale(ci.TwoLetterISOLanguageName);

            Locale locale = new Locale(ci.TwoLetterISOLanguageName);
            Control.TextLocale = locale;
            Resources.Configuration.SetLocale(locale);
            var config = new Configuration { Locale = locale };
            Forms.Context.Resources.UpdateConfiguration(config, Forms.Context.Resources.DisplayMetrics);

            AlertApp.Infrastructure.DatePickerNullable view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, year, month, day);

            _dialog.SetButton(ok, (sender, e) =>
            {
                SetDate(_dialog.DatePicker.DateTime);
                this.Element.Format = this.Element._format;
                this.Element.AssignValue();
            });
            _dialog.SetButton2(cancel, (sender, e) =>
            {
                //this.Element.CleanDate();
                //Control.Text = this.Element.Format;
                //Control.
            });
        }
    }
}