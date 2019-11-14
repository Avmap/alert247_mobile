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

        public event BackButtonPressEventHandler OnDeleteButton;
        public event NumberEnteredEventHandler OnNumberEntered;

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

        public void OnNumberEnter(string test)
        {
            if (OnNumberEntered != null)
            {
                OnNumberEntered(test);
            }
        }
    }
}
