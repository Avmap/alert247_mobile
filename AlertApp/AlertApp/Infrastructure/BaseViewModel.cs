using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private ICommand _BaseBackCommand;
        public ICommand BaseBackCommand
        {
            get
            {
                return _BaseBackCommand ?? (_BaseBackCommand = new Command(Back, () =>
                {
                    return !Busy;
                }));
            }
        }

        private async void Back()
        {
            await NavigationService.PopAsync(false);
        }

        public bool ShowContactsMenuButton => Preferences.Get(Settings.CONTACTS_BUTTON_VISIBLE, true);
        public bool ShowInfoMenuButton => Preferences.Get(Settings.INFORMATION_BUTTON_VISIBLE, true);

        public bool? IsVisible { get; set; }

        public bool NotBusy { get { return !Busy; } }

        private INavigation _NavigationService;
        public INavigation NavigationService
        {
            get { return _NavigationService; }
            set { _NavigationService = value; }
        }


        private string title = string.Empty;
        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        /// <value>The title.</value>
        public const string TitlePropertyName = "Title";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, TitlePropertyName); }
        }

        private string subTitle = string.Empty;
        /// <summary>
        /// Gets or sets the "Subtitle" property
        /// </summary>
        public const string SubtitlePropertyName = "Subtitle";
        public string Subtitle
        {
            get { return subTitle; }
            set { SetProperty(ref subTitle, value, SubtitlePropertyName); }
        }

        private string icon = null;
        /// <summary>
        /// Gets or sets the "Icon" of the viewmodel
        /// </summary>
        public const string IconPropertyName = "Icon";
        public string Icon
        {
            get { return icon; }
            set { SetProperty(ref icon, value, IconPropertyName); }
        }

        // here's your shared IsBusy property
        public const string BusyPropertyName = "Busy";
        private bool _Busy;
        public bool Busy
        {
            get { return _Busy; }
            set
            {
                SetProperty(ref _Busy, value, BusyPropertyName);
              OnPropertyChanged("NotBusy");
            }
        }

        private Page _Page;
        public Page Page
        {
            get { return _Page; }
            set
            {
                _Page = value;
                OnPropertyChanged("Page");
            }
        }

        protected void SetProperty<U>(
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {
            if (EqualityComparer<U>.Default.Equals(backingStore, value))
                return;

            if (onChanging != null)
                onChanging(value);

            OnPropertyChanging(propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
        }

        #region INotifyPropertyChanging implementation
        public event Xamarin.Forms.PropertyChangingEventHandler PropertyChanging;
        #endregion

        public void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging == null)
                return;

            PropertyChanging(this, new Xamarin.Forms.PropertyChangingEventArgs(propertyName));
        }


        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Task<string> DisplayActionSheet(string title, string[] listItems,string buttonText)
        {
            var test = Application.Current.MainPage.DisplayActionSheet(title, buttonText, null, listItems);
            if (test != null)
            {
                return test;
            }
            else
            {
                return null;
            }
        }
        protected void showOKMessage(string title, string message)
        {
            Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }


  
        protected async Task<bool> showAlertMessage(string title, string message, string accept, string cancel)
        {
            var result = await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
            return result;
        }

        protected Task<string> showInfoMessage(string title, string cancelButton)
        {
            var dialog = App.Current.MainPage.DisplayActionSheet(title, cancelButton, null, new string[] { cancelButton });
            if (dialog != null)
            {
                return dialog;
            }
            else
            {
                return null;
            }
        }

        protected Task<string> DisplayActionSheet(string title, string cancelString, string[] listItems)
        {
            var dialog = App.Current.MainPage.DisplayActionSheet(title, cancelString, null, listItems);
            if (dialog != null)
            {
                return dialog;
            }
            else
            {
                return null;
            }
        }
        public abstract void SetBusy(bool isBusy);

        protected string GetErrorDescription(Dictionary<string, string> labels)
        {
            if (labels == null)
                return "Unknown error. No error description.";

            if (labels.Keys.Count == 0)
                return "Unknown error. No error description.";

            var selectedlanguage = Preferences.Get(Settings.SelectedLanguage, Language.Codes.English);

            var errorDescription = labels[selectedlanguage];


            return errorDescription;
        }
    }
}
