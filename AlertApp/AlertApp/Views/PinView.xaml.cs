using AlertApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinView : ContentView
    {
        

        #region HideFrame (Bindable boolean)
        public static readonly BindableProperty HideFrameProperty = BindableProperty.Create(
                                                                  "HideFrame", //Public name to use
                                                                  typeof(bool), //this type
                                                                  typeof(PinView), //parent type (tihs control)
                                                                  false); //default value
        public bool HideFrame
        {
            get { return (bool)GetValue(HideFrameProperty); }
            set { SetValue(HideFrameProperty, value); }
        }
        #endregion HideFrame (Bindable boolean)

        private bool DeletePressed;

        public EntryCentered Pin1Entry => Pin1;
        public EntryCentered Pin2Entry => Pin2;
        public EntryCentered Pin3Entry => Pin3;
        public EntryCentered Pin4Entry => Pin4;

        public PinView()
        {
            InitializeComponent();

            Pin1.OnDeleteButton += Pin1_OnBackButton;
            Pin2.OnDeleteButton += Pin2_OnBackButton;
            Pin3.OnDeleteButton += Pin3_OnBackButton;
            Pin4.OnDeleteButton += Pin4_OnBackButton;

            Pin1.OnNumberEntered += Pin1_OnNumberEntered;
            Pin2.OnNumberEntered += Pin2_OnNumberEntered;
            Pin3.OnNumberEntered += Pin3_OnNumberEntered;
            Pin4.OnNumberEntered += Pin4_OnNumberEntered;

            

            if (Device.RuntimePlatform == Device.iOS)
            {
                Pin1.iOSFocused += Pin_iOSFocused;
                Pin2.iOSFocused += Pin_iOSFocused;
                Pin3.iOSFocused += Pin_iOSFocused;
                Pin4.iOSFocused += Pin_iOSFocused;
            }
            this.LayoutChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    GetFocus(Pin1);
                });
            };

        }

        public void focusControl()
        {
             
        }

        private void Pin_iOSFocused(object sender, Guid entryId)
        {
            Pin_Focused(sender, null);
        }

        private void Pin1_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin1.Text))
            {
                Pin1.Text = text;
                Pin2.Focus();
            }
            else
            {
                Pin2.Text = text;
                Pin2.Focus();
            }

        }
        private void Pin2_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin2.Text))
            {
                Pin2.Text = text;
                Pin3.Focus();
            }
            else
            {
                Pin3.Text = text;
                Pin3.Focus();
            }

        }
        private void Pin3_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin3.Text))
            {
                Pin3.Text = text;
                Pin4.Focus();
            }
            else
            {
                Pin4.Text = text;
                Pin4.Focus();
            }

        }
        private void Pin4_OnNumberEntered(string text)
        {
            if (string.IsNullOrWhiteSpace(Pin4.Text))
            {
                Pin4.Text = text;
            }
        }

        private void Pin1_OnBackButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            Pin1.Text = "";
        }

        private void Pin2_OnBackButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin2.Text) && Pin2.Text.Length == 1)
            {
                Pin2.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin2.Text))
            {
                Pin1.Text = "";
                Pin1.Focus();
            }
        }

        private void Pin3_OnBackButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin3.Text) && Pin3.Text.Length == 1)
            {
                Pin3.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin3.Text))
            {
                Pin2.Text = "";
                Pin2.Focus();
            }
        }
        private void Pin4_OnBackButton(object sender, EventArgs e)
        {
            DeletePressed = true;
            if (!string.IsNullOrWhiteSpace(Pin4.Text) && Pin4.Text.Length == 1)
            {
                Pin4.Focus();
                Pin4.Text = "";
            }
            else if (string.IsNullOrEmpty(Pin4.Text))
            {
                Pin3.Text = "";
                Pin3.Focus();
            }
        }

        private void Pin1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin2.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin3.Focus();
                }
                else
                {
                    Pin1.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    Pin4.Focus();
                }
                else
                {
                    Pin2.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.NewTextValue != null && e.NewTextValue.Length == 1)
                {
                    // Pin4.Focus();
                }
                else
                {
                    if (!string.IsNullOrEmpty(Pin3.Text))
                    {

                    }
                    Pin4.Focus();
                }
            }
            catch (Exception)
            {

            }

        }
        private void Pin_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                if (CheckFullPin())
                {
                    GetFocus(Pin4);
                    return;
                }

                var entry = sender as Entry;

                if (entry.Id == Pin1.Id)
                {
                    if (!string.IsNullOrWhiteSpace(Pin1.Text) && string.IsNullOrWhiteSpace(Pin2.Text) && !DeletePressed)
                    {
                        GetFocus(Pin2);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin2.Text) && string.IsNullOrWhiteSpace(Pin3.Text) && !DeletePressed)
                    {
                        GetFocus(Pin3);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin2.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(Pin2.Text) && string.IsNullOrWhiteSpace(Pin3.Text) && !DeletePressed)
                    {
                        GetFocus(Pin3);
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin3.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin2.Text))
                    {
                        GetFocus(Pin2);
                        return;
                    }

                    if (!string.IsNullOrWhiteSpace(Pin3.Text) && string.IsNullOrWhiteSpace(Pin4.Text) && !DeletePressed)
                    {
                        GetFocus(Pin4);
                    }
                }

                if (entry.Id == Pin4.Id)
                {
                    if (string.IsNullOrWhiteSpace(Pin1.Text))
                    {
                        GetFocus(Pin1);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin2.Text))
                    {
                        GetFocus(Pin2);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin3.Text))
                    {
                        GetFocus(Pin3);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(Pin4.Text))
                    {
                        GetFocus(Pin4);
                        return;
                    }
                }

            }
            catch (Exception)
            {

            }
            DeletePressed = false;
        }
        private bool CheckFullPin()
        {
            string userInput = String.Format("{0}{1}{2}{3}", Pin1.Text, Pin2.Text, Pin3.Text, Pin4.Text);
            if (userInput.Length == 4)
            {
                return true;
            }
            return false;
        }
        private void GetFocus(Entry entry)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                entry.Focus();
            });
        }

    }
}