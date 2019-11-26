using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AlertApp.Infrastructure;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.iOS.DependencyService.Storage))]
namespace AlertApp.iOS.DependencyService
{
    public class Storage : IStorage
    {
        public string GetFolderApp()
        {
            return "Alert24/7";
        }
        public string GetApplicationFolder()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return documents;

        }
        public string SaveFile(string filename, byte[] data)
        {
            try
            {
                var directoryname = Path.Combine(GetApplicationFolder(), "Alert247");

                //if (!Directory.Exists(directoryname))
                //    Directory.CreateDirectory(directoryname);

                var filePath = Path.Combine(GetApplicationFolder(), filename);

                if (!System.IO.Directory.Exists(GetApplicationFolder()))
                    System.IO.Directory.CreateDirectory(GetApplicationFolder());
                System.IO.File.WriteAllBytes(filePath, data);
                return filePath;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}