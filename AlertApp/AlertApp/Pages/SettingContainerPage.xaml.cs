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
	public partial class SettingContainerPage : ContentPage
	{
		public SettingContainerPage (string title,string subtitle,ContentView child)
		{
			InitializeComponent ();
            this.title.Text = title;
            this.subtitle.Text = subtitle;

            this.container.Children.Add(child);
		}
	}
}