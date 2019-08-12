using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Infrastructure
{
    public interface IContactProfileImageProvider
    {
        ImageSource GetProfileImage(string uri); 
    }
}
