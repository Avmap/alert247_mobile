using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Environment = Android.OS.Environment;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.Storage))]
namespace AlertApp.Droid.DependencyService
{
    public class Storage : IStorage
    {
        public string GetApplicationFolder()
        {
            var externalSdPath = Environment.ExternalStorageDirectory + "/Alert247/" + "files/";
            return externalSdPath;
        }

        public void SaveFile(string filename, byte[] data)
        {

            var filePath = Path.Combine(GetApplicationFolder(), filename);
            if (!System.IO.Directory.Exists(GetApplicationFolder()))
                System.IO.Directory.CreateDirectory(GetApplicationFolder());
            System.IO.File.WriteAllBytes(filePath, data);

        }
    }
}