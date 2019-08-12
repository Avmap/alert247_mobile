using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddContactPage : ContentPage
    {
        private int _lastDisappearedItemIndex { get; set; } = 0;

        public AddContactPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<AddContactPageViewModel>();
            contactList.ItemDisappearing += ContactList_ItemDisappearing;
            ToolbarItems.Add(new ToolbarItem
            {
                IconImageSource = "ic_action_name.png",                
                Command = ((AddContactPageViewModel)this.BindingContext).EnterNumberCommand,
            });
        }

        private void ContactList_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            if (_lastDisappearedItemIndex >= e.ItemIndex)
            {
                //scroll up
                // if (!fab.IsVisible)
                //  fab.IsVisible = true;
            }
            else
            {
                //scroll down
                // if (fab.IsVisible)
                // fab.IsVisible = false;
            }
            _lastDisappearedItemIndex = e.ItemIndex;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            if (cell.View.BackgroundColor == Color.Beige)
                cell.View.BackgroundColor = Color.White;
            else
                cell.View.BackgroundColor = Color.Beige;
        }

        private void Fab_Clicked(object sender, EventArgs e)
        {
            var vm = this.BindingContext as AddContactPageViewModel;
            vm.AddContactsCommand.Execute(null);
        }
    }
}