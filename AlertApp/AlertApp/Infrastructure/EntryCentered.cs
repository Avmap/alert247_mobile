using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public class EntryCentered : Entry
    {

        public delegate void BackButtonPressEventHandler(object sender, EventArgs e);
        public delegate void NumberEnteredEventHandler(string text);
        public delegate void iOSFocusEventHandler(object sender, Guid entryId);

        public event BackButtonPressEventHandler OnDeleteButton;
        public event NumberEnteredEventHandler OnNumberEntered;
        public event iOSFocusEventHandler iOSFocused;

        public bool IsDeletePressed { get; set; }
        public EntryCentered() { }

        public void OnDeletePress()
        {
            if (OnDeleteButton != null)
            {
                IsDeletePressed = true;
                OnDeleteButton(null, null);
                IsDeletePressed = false;
            }
        }

        public void OnNumberEnter(string text)
        {
            if (OnNumberEntered != null)
            {
                OnNumberEntered(text);
            }
        }

        public void OniOsFocused(object sender, Guid entryId)
        {
            if (iOSFocused != null)
            {
                iOSFocused(sender,entryId);
            }
        }
    }
}
