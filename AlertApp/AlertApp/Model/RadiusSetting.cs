using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Model
{
    public class RadiusSetting : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                _Checked = value;
                OnPropertyChanged("CheckedImageSource");
                OnPropertyChanged("Checked");
            }
        }


        public ImageSource CheckedImageSource
        {
            get
            {
                if (_Checked)
                    return ImageSource.FromFile("ic_radio_checked.png");

                return ImageSource.FromFile("ic_radio_unchecked.png");
            }
        }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

    }
}
