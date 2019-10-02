using AlertApp.Model.Api;
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
	public partial class CommunityRequestPage : ContentPage
	{
		public CommunityRequestPage (Contact contact)
		{
			InitializeComponent ();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("contact", contact);            
            this.BindingContext = ViewModelLocator.Instance.Resolve<CommunityRequestPageViewModel>(parameters);
        }

        protected override bool OnBackButtonPressed()
        {
            var vm = this.BindingContext as CommunityRequestPageViewModel;
            if (!vm.Busy)
                return base.OnBackButtonPressed();
            else
                return false;
        }
    }
}