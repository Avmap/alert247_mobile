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
	public partial class BlockedUsersPage : ContentView
    {
		public BlockedUsersPage ()
		{
			InitializeComponent ();
            this.BindingContext = ViewModelLocator.Instance.Resolve<BlockedUsersPageViewModel>();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}