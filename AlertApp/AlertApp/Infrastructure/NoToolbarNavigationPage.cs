using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
	public class NoToolbarNavigationPage : NavigationPage
	{
		public NoToolbarNavigationPage ()
		{
            SetHasNavigationBar(this, false);
		}
	}
}