using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AlertApp.Model
{
    public class LocationResult
    {
        public bool Ok { get; set; }
        public string ErroMessage { get; set; }
        public Location Location { get; set; }
    }
}
