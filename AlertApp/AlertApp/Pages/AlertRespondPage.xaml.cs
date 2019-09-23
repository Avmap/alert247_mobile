using AlertApp.Infrastructure;
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
	public partial class AlertRespondPage : ContentPage
	{
		public AlertRespondPage (NotificationAction notificationAction)
		{
			InitializeComponent ();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("notificationAction", notificationAction);
            this.BindingContext = ViewModelLocator.Instance.Resolve<AlertRespondPageViewModel>(parameters);
        }
       
    }
}