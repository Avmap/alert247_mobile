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

        public AddContactPage(List<string> currentContacts)
        {
            InitializeComponent();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("community", currentContacts);
            this.BindingContext = ViewModelLocator.Instance.Resolve<AddContactPageViewModel>(parameters);

            ToolbarItems.Add(new ToolbarItem
            {
                IconImageSource = "ic_action_name.png",
                Command = ((AddContactPageViewModel)this.BindingContext).EnterNumberCommand,
            });
        }

        private void Fab_Clicked(object sender, EventArgs e)
        {
            var vm = this.BindingContext as AddContactPageViewModel;
            vm.AddContactsCommand.Execute(null);
        }
    }
}