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
	public partial class DependandsPage : ContentPage
	{
		public DependandsPage ()
		{
			InitializeComponent ();
            this.BindingContext = Utils.ViewModelProvider.DependandsPageViewModel();
        }
	}
}